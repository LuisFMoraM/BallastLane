namespace Authorization
{
    /// <summary>
    /// Exposes constants to work with Authorization
    /// </summary>
    public class AuthorizationConstants
    {
        /// <summary>
        /// Gets the Security Key to Generate/Validate a JWT Token
        /// </summary>
        public const string SecurityKey = "245A4ECD3AAD271A8922369BAD529F78EAD4146B454F384DCED4BED136CE1C1F";

        /// <summary>
        /// Gets the Issuer to work with the Security Token
        /// </summary>
        public const string SecurityTokenIssuer = "PrescriberPoint";

        /// <summary>
        /// Gets the Audience to work with the Security Token
        /// </summary>
        public const string SecurityTokenAudience = "Readers";
    }
}