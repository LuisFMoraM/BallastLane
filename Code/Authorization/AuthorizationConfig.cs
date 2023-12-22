using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Authorization
{
    public class AuthorizationConfig : IAuthorizationConfig
    {
        /// <summary>
        /// Implementation of the <see cref="IAuthorizationConfig.JsonWebToken"/>
        /// </summary>
        public string JsonWebToken()
        {
            var securityKey = AuthorizationConstants.SecurityKey;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: AuthorizationConstants.SecurityTokenIssuer,
                audience: AuthorizationConstants.SecurityTokenAudience,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
