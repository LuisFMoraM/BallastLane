namespace BusinessLogic.Models
{
    /// <summary>
    /// Exposes data for a User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user's identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's phone
        /// </summary>
        public string Phone { get; set; }
    }
}
