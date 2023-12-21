using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Manages User actions in the DB
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user searching by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User info</returns>
        Task<User?> GetByEmail(string email);

        /// <summary>
        /// Adds a User to the database
        /// </summary>
        /// <param name="entity">User info</param>
        Task Add(User entity);
    }
}
