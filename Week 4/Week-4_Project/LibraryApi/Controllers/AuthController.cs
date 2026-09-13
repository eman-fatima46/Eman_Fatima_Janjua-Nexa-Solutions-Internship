using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace LibraryApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LibraryDbContext _context;

    private readonly IConfiguration _configuration;


    public AuthController(
        LibraryDbContext context,
        IConfiguration configuration)
    {
        _context = context;

        _configuration = configuration;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterDto dto)
    {
        bool usernameExists =
            await _context.Users.AnyAsync(
                u =>
                    u.Username
                    == dto.Username
            );


        if (usernameExists)
        {
            return BadRequest(
                "Username already exists."
            );
        }


        var passwordHasher =
            new PasswordHasher<User>();


        var user =
            new User
            {
                Username =
                    dto.Username,

                Role =
                    "User"
            };


        user.PasswordHash =
            passwordHasher.HashPassword(
                user,
                dto.Password
            );


        _context.Users.Add(user);

        await _context.SaveChangesAsync();


        return Ok(
            new
            {
                message =
                    "User registered successfully."
            }
        );
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginDto dto)
    {
        User? user =
            await _context.Users
                .SingleOrDefaultAsync(
                    u =>
                        u.Username
                        == dto.Username
                );


        if (user == null)
        {
            return Unauthorized(
                "Invalid username or password."
            );
        }


        var passwordHasher =
            new PasswordHasher<User>();


        PasswordVerificationResult result =
            passwordHasher
                .VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password
                );


        if (
            result
            == PasswordVerificationResult.Failed
        )
        {
            return Unauthorized(
                "Invalid username or password."
            );
        }


        string token =
            GenerateJwt(user);


        return Ok(
            new
            {
                token
            }
        );
    }


    private string GenerateJwt(
        User user)
    {
        string? jwtKey =
            _configuration["Jwt:Key"];

        string? issuer =
            _configuration["Jwt:Issuer"];

        string? audience =
            _configuration["Jwt:Audience"];

        int expiryMinutes =
            int.TryParse(
                _configuration[
                    "Jwt:ExpiryMinutes"
                ],
                out int parsedExpiry
            )
                ? parsedExpiry
                : 60;


        if (
            string.IsNullOrWhiteSpace(jwtKey)
        )
        {
            throw new InvalidOperationException(
                "JWT key is not configured."
            );
        }


        var claims =
            new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Username
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                )
            };


        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtKey
                )
            );


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms
                    .HmacSha256
            );


        var token =
            new JwtSecurityToken(
                issuer:
                    issuer,

                audience:
                    audience,

                claims:
                    claims,

                expires:
                    DateTime.UtcNow
                        .AddMinutes(
                            expiryMinutes
                        ),

                signingCredentials:
                    credentials
            );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}