using System.ComponentModel.DataAnnotations;

namespace TicketManager.DTOs.Auth;

public class RegisterDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string Name { get; set; } = null!;
    [Required] public string Lastname { get; set; } = null!;
}