using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    /// <summary>
    /// Define behaviors related to Users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds a User to the system
        /// </summary>
        /// <param name="entity">User info</param>
        Task Add(User entity);

        /// <summary>
        /// Validates info to allow the user to log in to the system
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>User info</returns>
        Task<User> LogIn(string email, string password);
    }
}
