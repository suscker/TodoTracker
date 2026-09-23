using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoTracker.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TodoTracker.Services;
public class JwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(User user)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"]!);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("login", user.Login)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials,
            claims: claims
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}