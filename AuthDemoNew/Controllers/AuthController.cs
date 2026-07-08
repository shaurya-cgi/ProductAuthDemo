using AuthDemo.Data;
using AuthDemoNew.Data;
using AuthDemoNew.Dtos;
using AuthDemoNew.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "Healthy" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var existingUser = _context.Users
            .FirstOrDefault(x => x.Username == registerDto.Username);

        if (existingUser != null)
        {
            return BadRequest(new
            {
                message = "Username already exists"
            });
        }

        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.PasswordHash),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "User registered successfully"
        });
    }
}