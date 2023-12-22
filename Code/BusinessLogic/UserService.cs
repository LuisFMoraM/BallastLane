using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic
{
    public class UserService : IUserService
    {
        public const string ExistingUser = "There is already a User with the same Email";
        public const string NonExistingUser = "This User does not exist - Create it if needed!";
        public const string InvalidCredentials = "Email or Password are invalid. Try again!";

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Implementation of the <see cref="IUserService.Add(User)"/>
        /// </summary>
        public async Task Add(User entity)
        {
            var existingUser = await _userRepository.GetByEmail(entity.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException(ExistingUser);
            }

            var dbEntity = _mapper.Map<DataAccess.Entities.User>(entity);
            await _userRepository.Add(dbEntity);
        }

        /// <summary>
        /// Implementation of the <see cref="IUserService.LogIn(string, string)"/>
        /// </summary>
        public async Task<User> LogIn(string email, string password)
        {
            var dbUser = await _userRepository.GetByEmail(email);
            if (dbUser is null)
            {
                throw new InvalidOperationException(NonExistingUser);
            }

            if (dbUser.Password != password)
            {
                throw new ArgumentException(InvalidCredentials);
            }

            dbUser.Password = string.Empty;
            return _mapper.Map<User>(dbUser);
        }
    }
}
