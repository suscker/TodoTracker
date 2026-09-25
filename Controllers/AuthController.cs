using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTracker.Models;
using TodoTracker.Models.Dto;
using TodoTracker.Data;
using TodoTracker.Services;


namespace TodoTracker.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly JwtService _jwtService;
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    public AuthController(AppDbContext db, IPasswordHasher<User> passwordHasher, JwtService jwtService, IConfiguration configuration)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest loginRequest)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == loginRequest.Login);
        if(user == null)
        {
            return Unauthorized();
        }
        var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);
        if(passwordCheck == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }
        var token = _jwtService.GenerateToken(user);
        var cookieOptions = new CookieOptions
        {
          HttpOnly = true,
          Secure = false,
          SameSite = SameSiteMode.Lax,
          Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiryMinutes")),
          Path = "/"
          
        };
        Response.Cookies.Append("access_token", token, cookieOptions);
        return UserToDto(user);
    }
    private static UserDto UserToDto(User user) => new UserDto
    {
        Id = user.Id,
        Login = user.Login,
        Name = user.Name,
        RegisteredAt = user.RegisteredAt
    };
}