using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pets_care.Models;

namespace pets_care.Auth
{
    public class TokenGenerator : ITokenGenerator
    {
        /// <summary>
        /// This function is to Generate Token 
        /// </summary>
        /// <returns>A string, the token JWT</returns>
        public string Generate(Client client)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
       
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = AddClaims(client),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY"))),
                        SecurityAlgorithms.HmacSha256Signature
                ),
                Expires = DateTime.Now.AddHours(8)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Function that adds the claims to the token
        /// </summary>
        /// <param name="client"> A client object value</param>
        /// <returns>Returns an object of type ClaimsIdentity</returns>
        public ClaimsIdentity AddClaims(Client client)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim("Id", client.ClientId.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, client.Name));
            claims.AddClaim(new Claim(ClaimTypes.Email, client.Email));
            claims.AddClaim(new Claim(ClaimTypes.Role, client.Role));
            claims.AddClaim(new Claim("Expiration Date:", DateTime.Now.AddHours(8).ToString()));

            return claims;
        }
    }
}