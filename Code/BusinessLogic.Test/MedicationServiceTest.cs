using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Repositories.Interfaces;
using MedicationApi.MapperProfiles;
using Moq;

namespace BusinessLogic.Test
{
    public class MedicationServiceTest : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMedicationRepository> _repositoryMock;
        private readonly MedicationService _service;

        public MedicationServiceTest()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MedicationProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            _repositoryMock = new Mock<IMedicationRepository>(MockBehavior.Strict);
            _service = new MedicationService(_mapper, _repositoryMock.Object);
        }

        public void Dispose()
        {
            _repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task MedicationService_GetAll_ReturnsRecords()
        {
            // Arrange
            var medList = new List<DataAccess.Entities.Medication>
            {
                new DataAccess.Entities.Medication { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 },
                new DataAccess.Entities.Medication { Id = 2, Name = "BodyCare", Brand = "Care", Price = 5 }
            };

            _repositoryMock.Setup(r => r.GetAll())
                .ReturnsAsync(medList);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.Equal(medList.Count, result.Count);
            Assert.Equal(medList[0].Id, result[0].Id);
            Assert.Equal(medList[0].Name, result[0].Name);
        }

        [Fact]
        public async Task MedicationService_GetById_WhenRecordDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var medicationId = 1;
            _repositoryMock.Setup(r => r.GetById(medicationId))
                .ReturnsAsync(null as DataAccess.Entities.Medication);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetById(medicationId));
            Assert.Equal(MedicationService.NonExistingMedication, errorMsg.Message);
        }

        [Fact]
        public async Task MedicationService_GetById_WhenRecordExists_ReturnsMedicationData()
        {
            // Arrange
            var medicationId = 1;
            var dbMedication = new DataAccess.Entities.Medication { Id = 1, Name = "MedicationTest", Brand = "Care" };
            _repositoryMock.Setup(r => r.GetById(medicationId)).ReturnsAsync(dbMedication);

            // Act
            var medication = await _service.GetById(medicationId);

            // Assert
            Assert.NotNull(medication);
            Assert.Equal(medicationId, medication.Id);
            Assert.Equal(dbMedication.Name, medication.Name);
            Assert.Equal(dbMedication.Brand, medication.Brand);
        }

        [Fact]
        public async Task MedicationService_Add_WhenRecordExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var medList = new List<DataAccess.Entities.Medication>
            {
                new DataAccess.Entities.Medication { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 },
                new DataAccess.Entities.Medication { Id = 2, Name = "BodyCare", Brand = "Care", Price = 5 }
            };
            var recordToAdd = new DataAccess.Entities.Medication { Id = 2, Name = "BodyCare", Brand = "Care", Price = 15 };
            var model = _mapper.Map<Medication>(recordToAdd);

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(medList);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Add(model));
            Assert.Equal(MedicationService.ExistingMedication, errorMsg.Message);
        }

        [Fact]
        public async Task MedicationService_Add_WhenRecordDoesNotExist_CreatesMedication()
        {
            // Arrange
            var medList = new List<DataAccess.Entities.Medication>
            {
                new DataAccess.Entities.Medication { Id = 1, Name = "FaceAcne", Brand = "Free", Price = 10 },
                new DataAccess.Entities.Medication { Id = 2, Name = "BodyCare", Brand = "Care", Price = 5 }
            };
            var recordToAdd = new DataAccess.Entities.Medication { Id = 3, Name = "LegCare", Brand = "Care", Price = 15 };
            var model = _mapper.Map<Medication>(recordToAdd);

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(medList);
            _repositoryMock.Setup(r => r.Add(It.IsAny<DataAccess.Entities.Medication>())).Returns(Task.CompletedTask);

            // Act
            await _service.Add(model);

            // Assert
            _repositoryMock.Verify(r => r.Add(It.Is<DataAccess.Entities.Medication>(m =>
                m.Id == recordToAdd.Id &&
                m.Name == recordToAdd.Name &&
                m.Brand == recordToAdd.Brand &&
                m.Price == recordToAdd.Price)),
            Times.Once);
        }

        [Fact]
        public async Task MedicationService_Update_WhenRecordDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var recordToUpdate = new DataAccess.Entities.Medication { Id = 3, Name = "LegCare", Brand = "Care", Price = 15 };
            var model = _mapper.Map<Medication>(recordToUpdate);
            _repositoryMock.Setup(r => r.GetById(model.Id)).ReturnsAsync(null as DataAccess.Entities.Medication);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Update(model.Id, model));
            Assert.Equal(MedicationService.NonExistingMedication, errorMsg.Message);
        }

        [Fact]
        public async Task MedicationService_Update_WhenRecordExists_UpdatesInfo()
        {
            // Arrange
            var existingRecord = new DataAccess.Entities.Medication { Id = 3, Name = "LegCare", Brand = "Care", Price = 5 };
            var recordToUpdate = new DataAccess.Entities.Medication { Id = 3, Name = "LegCare", Brand = "Care", Price = 15 };
            var model = _mapper.Map<Medication>(recordToUpdate);
            _repositoryMock.Setup(r => r.GetById(model.Id)).ReturnsAsync(existingRecord);
            _repositoryMock.Setup(r => r.Update(It.IsAny<DataAccess.Entities.Medication>())).Returns(Task.CompletedTask);

            // Act
            await _service.Update(model.Id, model);

            _repositoryMock.Verify(r => r.Update(It.Is<DataAccess.Entities.Medication>(m =>
                m.Id == recordToUpdate.Id &&
                m.Name == recordToUpdate.Name &&
                m.Brand == recordToUpdate.Brand &&
                m.Price == recordToUpdate.Price)),
            Times.Once);
        }

        [Fact]
        public async Task MedicationService_Delete_WhenRecordDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var recordToDelete = new DataAccess.Entities.Medication { Id = 4, Name = "ArmCare", Brand = "Care", Price = 8 };
            _repositoryMock.Setup(r => r.GetById(recordToDelete.Id)).ReturnsAsync(null as DataAccess.Entities.Medication);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Delete(recordToDelete.Id));
            Assert.Equal(MedicationService.NonExistingMedication, errorMsg.Message);
        }

        [Fact]
        public async Task MedicationService_Delete_WhenRecordExists_RemovesMedication()
        {
            // Arrange
            var recordToDelete = new DataAccess.Entities.Medication { Id = 4, Name = "ArmCare", Brand = "Care", Price = 8 };
            _repositoryMock.Setup(r => r.GetById(recordToDelete.Id)).ReturnsAsync(recordToDelete);
            _repositoryMock.Setup(r => r.Delete(It.IsAny<DataAccess.Entities.Medication>())).Returns(Task.CompletedTask);

            // Act
            await _service.Delete(recordToDelete.Id);

            _repositoryMock.Verify(r => r.Delete(It.Is<DataAccess.Entities.Medication>(m =>
                m.Id == recordToDelete.Id &&
                m.Name == recordToDelete.Name &&
                m.Brand == recordToDelete.Brand &&
                m.Price == recordToDelete.Price)),
            Times.Once);
        }

    }
}