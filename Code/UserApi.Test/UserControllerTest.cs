using Authorization;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UserApi.Controllers;
using UserApi.DTOs;
using UserApi.MapperProfiles;

namespace UserApi.Test
{
    public class UserControllerTest : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserService> _serviceMock;
        private readonly Mock<IAuthorizationConfig> _authConfigMock;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            var _logger = new NullLogger<UserController>();
            _authConfigMock = new Mock<IAuthorizationConfig>(MockBehavior.Strict);
            _serviceMock = new Mock<IUserService>(MockBehavior.Strict);
            _controller = new UserController(_logger, _mapper, _authConfigMock.Object, _serviceMock.Object);
        }

        public void Dispose()
        {
            _authConfigMock.VerifyAll();
            _serviceMock.VerifyAll();
        }

        [Fact]
        public async Task UserController_Add_WhenThrowsInvalidOperationException_ReturnsBadRequest()
        {
            // Arrange
            var user = new UserDto { Id = 1, Name = "TestUser", Email = "test@email.com" };
            var excMsg = "The User exists already";
            _serviceMock.Setup(s => s.Add(It.Is<User>(u => u.Id == user.Id && u.Name == user.Name)))
                .ThrowsAsync(new InvalidOperationException(excMsg));

            // Act
            var response = await _controller.Add(user);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(excMsg, result.Value);
        }

        [Fact]
        public async Task UserController_Add_WhenThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var user = new UserDto { Id = 1, Name = "TestUser", Email = "test@email.com" };
            _serviceMock.Setup(s => s.Add(It.Is<User>(u => u.Id == user.Id && u.Name == user.Name)))
                .ThrowsAsync(new Exception());

            // Act
            var response = await _controller.Add(user);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task UserController_Add_WhenCreatesANewUser_ReturnsCreatedStatus()
        {
            // Arrange
            var user = new UserDto { Id = 1, Name = "TestUser", Email = "test@email.com" };
            _serviceMock.Setup(s => s.Add(It.Is<User>(u => 
                u.Id == user.Id && 
                u.Name == user.Name && 
                u.Email == user.Email)))
            .Returns(Task.CompletedTask);

            // Act
            var response = await _controller.Add(user);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task UserController_Login_WhenThrowsInvalidOperationException_ReturnsBadRequest()
        {
            // Arrange
            var email = "test@email.com";
            var loginInfo = new LoginDto { Password = "12345" };
            var excMsg = "The User does not exist";
            _serviceMock.Setup(s => s.LogIn(email, loginInfo.Password))
                .ThrowsAsync(new InvalidOperationException(excMsg));

            // Act
            var response = await _controller.Login(email, loginInfo);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(excMsg, result.Value);
        }

        [Fact]
        public async Task UserController_Login_WhenThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var email = "test@email.com";
            var loginInfo = new LoginDto { Password = "12345" };
            var excMsg = "Email or Password are invalid.";
            _serviceMock.Setup(s => s.LogIn(email, loginInfo.Password))
                .ThrowsAsync(new ArgumentException(excMsg));

            // Act
            var response = await _controller.Login(email, loginInfo);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(excMsg, result.Value);
        }

        [Fact]
        public async Task UserController_Login_WhenThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var email = "test@email.com";
            var loginInfo = new LoginDto { Password = "12345" };
            _serviceMock.Setup(s => s.LogIn(email, loginInfo.Password))
                .ThrowsAsync(new Exception());

            // Act
            var response = await _controller.Login(email, loginInfo);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task UserController_Login_WhenLogInSuccessfully_ReturnsUserData()
        {
            // Arrange
            var email = "test@email.com";
            var loginInfo = new LoginDto { Password = "12345" };

            var userInfo = new User { Id = 1, Name = "TestUser", Email = email };
            _serviceMock.Setup(s => s.LogIn(email, loginInfo.Password))
                .ReturnsAsync(userInfo);

            var token = Guid.NewGuid().ToString();
            _authConfigMock.Setup(a => a.JsonWebToken()).Returns(token);

            // Act
            var response = await _controller.Login(email, loginInfo);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            var userDto = Assert.IsType<UserDto>(result.Value);
            Assert.Equal(userInfo.Id, userDto.Id);
            Assert.Equal(userInfo.Name, userDto.Name);
            Assert.Equal(userInfo.Email, userDto.Email);
            Assert.Equal(token, userDto.Token);
        }
    }
}