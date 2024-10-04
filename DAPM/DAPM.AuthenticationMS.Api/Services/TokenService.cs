using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DAPM.AuthenticationMS.Api.Services;

public class TokenService : ITokenService
{
    public readonly UserManager<IdentityUser> _userManager;

    public TokenService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> CreateToken(IdentityUser user)
    {

        bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");


        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("isAdmin", isAdmin.ToString().ToLower())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey??????????????????????????"));
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
        return tokenHandler.WriteToken(token);
    }
}
