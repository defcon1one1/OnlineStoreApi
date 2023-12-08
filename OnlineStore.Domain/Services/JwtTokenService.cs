using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace OnlineStore.Domain.Services;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJwtToken(User? user)
    {
        if (user is null) return string.Empty;
#pragma warning disable CS8604 // Possible null reference argument.
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
#pragma warning restore CS8604 // Possible null reference argument.
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        // Include user roles in the claims
        var claims = new List<System.Security.Claims.Claim>
        {
            new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(System.Security.Claims.ClaimTypes.Email, user.Email),
            new(System.Security.Claims.ClaimTypes.Role, user.Role.ToString())
        };

        DateTime expireTime = DateTime.Now.AddHours(1);
        JwtSecurityToken token = new(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: expireTime,
            signingCredentials: credentials);

        JwtSecurityTokenHandler tokenHandler = new();
        return tokenHandler.WriteToken(token);
    }
}
