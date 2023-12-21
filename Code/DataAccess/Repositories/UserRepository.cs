using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using LinqToDB;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<User> _repository;
        private readonly AppDataConnection _connection;

        public UserRepository(
            IRepository<User> repository,
            AppDataConnection connection)
        {
            _repository = repository;
            _connection = connection;
        }

        /// <summary>
        /// Implementation of the <see cref="IUserRepository.GetByEmail(string)"/>
        /// </summary>
        public async Task<User?> GetByEmail(string email)
        {
            var user = await _connection.Users
                .SingleOrDefaultAsync(user => user.Email == email);

            if (user != null)
            {
                user.Password = EncryptionManager.Decrypt(user.Password);
            }

            return user;
        }

        /// <summary>
        /// Implementation of the <see cref="IUserRepository.Add(User)"/>
        /// </summary>
        public async Task Add(User entity)
        {
            entity.Password = EncryptionManager.Encrypt(entity.Password);
            await _repository.Add(entity);
        }
    }
}
