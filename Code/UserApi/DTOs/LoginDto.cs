using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs
{
    /// <summary>
    /// Exposes data for the Login info
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
