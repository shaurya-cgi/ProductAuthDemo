using AuthDemoNew.Data;
using AuthDemoNew.Dtos;
using AuthDemoNew.Models;
using AuthDemoNew.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "Healthy" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == registerDto.Username);

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

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "User registered successfully"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == loginDto.Username);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid username or password"
            });
        }

        bool isValidPassword =
            BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash
            );

        if (!isValidPassword)
        {
            return Unauthorized(new
            {
                message = "Invalid username or password"
            });
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token,
            username = user.Username
        });
    }
}