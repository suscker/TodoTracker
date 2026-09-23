using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTracker.Models;
using TodoTracker.Models.Dto;
using TodoTracker.Data;


namespace TodoTracker.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    public AuthController(AppDbContext db, IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
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

        return new LoginResponse{Token = "stub"};
    }

}