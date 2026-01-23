using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManager.DTOs.Auth;
using TicketManager.Models;

namespace TicketManager.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserManager<User> userManager) : ControllerBase 
{
    private readonly UserManager<User> _userManager = userManager;

    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            Name = dto.Name,
            Lastname = dto.Lastname
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "CUSTOMER");

        return Ok(new { message = "User registered successfully" });
    }
}