namespace Authorization
{
    public interface IAuthorizationConfig
    {
        /// <summary>
        /// Gets a JWT token to work with Authorize methods
        /// </summary>
        /// <returns>New JWT Token</returns>
        string JsonWebToken();
    }
}
