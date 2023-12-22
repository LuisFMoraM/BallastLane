using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using MedicationApi.Controllers;
using MedicationApi.DTOs;
using MedicationApi.MapperProfiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace MedicationApi.Test
{
    public class MedicationControllerTest : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMedicationService> _serviceMock;
        private readonly MedicationController _controller;

        public MedicationControllerTest()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MedicationProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            var _logger = new NullLogger<MedicationController>();
            _serviceMock = new Mock<IMedicationService>(MockBehavior.Strict);
            _controller = new MedicationController(_logger, _mapper, _serviceMock.Object);
        }

        public void Dispose()
        {
            _serviceMock.VerifyAll();
        }

        [Fact]
        public async Task MedicationController_Get_ReturnsRecords()
        {
            // Arrange
            var medList = new List<Medication>
            {
                new Medication { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 },
                new Medication { Id = 2, Name = "BodyCare", Brand = "Care", Price = 5 }
            };

            _serviceMock.Setup(s => s.GetAll()).ReturnsAsync(medList);

            // Act
            var response = await _controller.GetAll();

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            var records = Assert.IsType<List<MedicationDto>>(result.Value);
            Assert.Equal(medList.Count, records.Count);
            Assert.Equal(medList[0].Id, records[0].Id);
            Assert.Equal(medList[0].Name, records[0].Name);
            Assert.Equal(medList[1].Id, records[1].Id);
            Assert.Equal(medList[1].Name, records[1].Name);
        }

        [Fact]
        public async Task MedicationController_Add_WhenThrowsInvalidOperationException_ReturnsBadRequest()
        {
            // Arrange
            var medication = new MedicationDto { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 };
            var excMsg = "The Medication exists already";
            _serviceMock.Setup(s => s.Add(It.Is<Medication>(m => m.Id == medication.Id && m.Name == medication.Name)))
                .ThrowsAsync(new InvalidOperationException(excMsg));

            // Act
            var response = await _controller.Add(medication);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(excMsg, result.Value);
        }

        [Fact]
        public async Task MedicationController_Add_WhenThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var medication = new MedicationDto { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 };
            _serviceMock.Setup(s => s.Add(It.Is<Medication>(m => m.Id == medication.Id && m.Name == medication.Name)))
                .ThrowsAsync(new Exception());

            // Act
            var response = await _controller.Add(medication);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task MedicationController_Add_WhenCreatesANewRecord_ReturnsCreatedStatus()
        {
            // Arrange
            var medication = new MedicationDto { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 };
            _serviceMock.Setup(s => s.Add(It.Is<Medication>(m => m.Id == medication.Id && m.Name == medication.Name)))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _controller.Add(medication);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task MedicationController_Update_WhenThrowsInvalidOperationException_ReturnsBadRequest()
        {
            // Arrange
            var medication = new MedicationDto { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 };
            var excMsg = "The Medication does not exist";
            _serviceMock.Setup(s => s.Update(medication.Id, It.Is<Medication>(m => m.Id == medication.Id && m.Name == medication.Name)))
                .ThrowsAsync(new InvalidOperationException(excMsg));

            // Act
            var response = await _controller.Update(medication.Id, medication);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(excMsg, result.Value);
        }

        [Fact]
        public async Task MedicationController_Update_WhenThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var medication = new MedicationDto { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 };
            _serviceMock.Setup(s => s.Update(medication.Id, It.Is<Medication>(m => m.Id == medication.Id && m.Name == medication.Name)))
                .ThrowsAsync(new Exception());

            // Act
            var response = await _controller.Update(medication.Id, medication);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task MedicationController_Update_WhenUpdatesARecord_ReturnsOkStatus()
        {
            // Arrange
            var medication = new MedicationDto { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 };
            _serviceMock.Setup(s => s.Update(medication.Id, It.Is<Medication>(m => m.Id == medication.Id && m.Name == medication.Name)))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _controller.Update(medication.Id, medication);

            // Assert
            var result = Assert.IsType<OkResult>(response);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task MedicationController_Delete_WhenThrowsInvalidOperationException_ReturnsBadRequest()
        {
            // Arrange
            var medicationId = 1;
            var excMsg = "The Medication does not exist";
            _serviceMock.Setup(s => s.Delete(medicationId)).ThrowsAsync(new InvalidOperationException(excMsg));

            // Act
            var response = await _controller.Delete(medicationId);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(excMsg, result.Value);
        }

        [Fact]
        public async Task MedicationController_Delete_WhenThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var medicationId = 1;
            _serviceMock.Setup(s => s.Delete(medicationId)).ThrowsAsync(new Exception());

            // Act
            var response = await _controller.Delete(medicationId);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task MedicationController_Delete_WhenDeletesARecord_ReturnsOkStatus()
        {
            // Arrange
            var medicationId = 1;
            _serviceMock.Setup(s => s.Delete(medicationId)).Returns(Task.CompletedTask);

            // Act
            var response = await _controller.Delete(medicationId);

            // Assert
            var result = Assert.IsType<OkResult>(response);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}