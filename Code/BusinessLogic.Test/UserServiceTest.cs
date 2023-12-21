using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Repositories.Interfaces;
using MedicationApi.MapperProfiles;
using Moq;

namespace BusinessLogic.Test
{
    public class UserServiceTest : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UserService _service;

        public UserServiceTest()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            _repositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _service = new UserService(_mapper, _repositoryMock.Object);
        }

        public void Dispose()
        {
            _repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task UserService_Add_WhenRecordExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var user = new User { Email = "test@email.com" };
            var dbUser = _mapper.Map<DataAccess.Entities.User>(user);
            _repositoryMock.Setup(r => r.GetByEmail(user.Email)).ReturnsAsync(dbUser);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Add(user));
            Assert.Equal(UserService.ExistingUser, errorMsg.Message);
        }

        [Fact]
        public async Task UserService_Add_WhenRecordDoesNotExist_CreatesUser()
        {
            // Arrange
            var user = new User { Name = "NewUser", Email = "test@email.com", Phone = "12345" };
            _repositoryMock.Setup(r => r.GetByEmail(user.Email)).ReturnsAsync(null as DataAccess.Entities.User);
            _repositoryMock.Setup(r => r.Add(It.IsAny<DataAccess.Entities.User>())).Returns(Task.CompletedTask);

            // Act
            await _service.Add(user);

            // Assert
            _repositoryMock.Verify(r => r.Add(It.Is<DataAccess.Entities.User>(u =>
                u.Name == user.Name &&
                u.Email == user.Email &&
                u.Phone == user.Phone)),
            Times.Once);
        }

        [Fact]
        public async Task UserService_LogIn_WhenRecordDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var email = "test@email.com";
            _repositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(null as DataAccess.Entities.User);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.LogIn(email, "12345"));
            Assert.Equal(UserService.NonExistingUser, errorMsg.Message);
        }

        [Fact]
        public async Task UserService_LogIn_WhenPasswordIsInvalid_ThrowsArgumentException()
        {
            // Arrange
            var email = "test@email.com";
            var psw = "12345";
            var dbRecord = new DataAccess.Entities.User { Email = email, Password = "1223355" };
            _repositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(dbRecord);

            // Act & Assert
            var errorMsg = await Assert.ThrowsAsync<ArgumentException>(() => _service.LogIn(email, psw));
            Assert.Equal(UserService.InvalidCredentials, errorMsg.Message);
        }

        [Fact]
        public async Task UserService_LogIn_WhenCredentialsAreValid_ReturnsUserInfo()
        {
            // Arrange
            var email = "test@email.com";
            var psw = "12345";
            var dbRecord = new DataAccess.Entities.User {Id = 1, Name = "TestUser", Email = email, Password = psw };
            _repositoryMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(dbRecord);

            // Act
            var loggedUser = await _service.LogIn(email, psw);

            // Assert
            Assert.Equal(loggedUser.Id, dbRecord.Id);
            Assert.Equal(loggedUser.Name, dbRecord.Name);
            Assert.Equal(loggedUser.Email, dbRecord.Email);
        }
    }
}
