using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTracker.Models;
using TodoTracker.Models.Dto;
using TodoTracker.Data;
using Microsoft.AspNetCore.Identity;

namespace TodoTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UsersController(AppDbContext db, IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(RegisterUserRequest registerUserRequest)
    {
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Name = registerUserRequest.Name,
            RegisteredAt = DateTime.UtcNow,
            Login = registerUserRequest.Login
        };

        bool loginExist = await _db.Users.AnyAsync(u => u.Login == user.Login);
        if(loginExist)
        {
            return Problem(
                detail: "Пользователь с таким логином уже существует",
                statusCode: StatusCodes.Status409Conflict,
                title: "Логин занят"
                );
        }
        

        user.PasswordHash = _passwordHasher.HashPassword(user, registerUserRequest.Password);



        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
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