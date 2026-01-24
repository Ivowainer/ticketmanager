using System.ComponentModel.DataAnnotations;

namespace TicketManager.DTOs.Message;

public class CreateMessageDto
{
    [Required]
    [StringLength(100, ErrorMessage = "The message must not exceed 1000 characters")]
    public string Content { get; set; } = null!;
}