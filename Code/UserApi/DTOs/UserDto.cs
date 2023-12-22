using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs
{
    /// <summary>
    /// Exposes data for a User
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the user's identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user's name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's phone
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets a Token to call other services in the system
        /// </summary>
        public string? Token { get; set; }
    }
}
