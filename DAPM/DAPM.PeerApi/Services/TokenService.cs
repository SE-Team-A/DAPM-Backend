using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly string TOKEN_KEY = "secretkey?????????????????????????!";

        public bool checkSignature(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Set to true if you want to validate the issuer
                ValidateAudience = false, // Set to true if you want to validate the audience
                ValidateLifetime = false, // Set to true if you want to validate token expiration
                ValidateIssuerSigningKey = true, // Ensure the signing key is validated
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TOKEN_KEY)), // Use the symmetric key for validation
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Validate the token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                return true;
            }
            catch (SecurityTokenException ex)
            {
                return false;
            }
        }

        public string createToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TOKEN_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds,
                // Issuer = _configuration["Jwt:Issuer"],  // Optional: Set issuer
                // Audience = _configuration["Jwt:Audience"] // Optional: Set audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}