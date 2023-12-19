using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;

namespace EvaExchange.API.Infrastructure.Services;

public interface ITokenService
{
    TokenModel GenerateToken(User user);
}

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly SymmetricSecurityKey _jwtSymmetricSecurityKey = new(
        Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is null")));

    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    public TokenModel GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var expiresIn = DateTime.UtcNow.AddMinutes(100);
        var token = new JwtSecurityToken(
            expires: expiresIn,
            claims: claims,
            signingCredentials: new SigningCredentials(_jwtSymmetricSecurityKey, SecurityAlgorithm)
        );

        return new TokenModel(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresIn);
    }
}