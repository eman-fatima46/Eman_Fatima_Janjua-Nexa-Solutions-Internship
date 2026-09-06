using LibraryApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(
        LoginRequestDto request)
    {
        if (
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password)
        )
        {
            return BadRequest(
                "Email and password are required."
            );
        }

        return Ok(
            new
            {
                message =
                    "Login endpoint skeleton is working. "
                    + "Password verification and JWT issuing "
                    + "will be implemented in Week 4."
            }
        );
    }
}