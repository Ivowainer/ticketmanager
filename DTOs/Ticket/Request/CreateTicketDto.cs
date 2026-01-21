using System.ComponentModel.DataAnnotations;

namespace TicketManager.DTOs.Ticket.Request;

public class CreateTicketDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title mujst be between 5 and 200 characters")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = null!;
}