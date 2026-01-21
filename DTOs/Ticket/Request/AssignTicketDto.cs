using System.ComponentModel.DataAnnotations;

namespace TicketManager.DTOs.Ticket.Request;

public class AssignTicketDto
{
    [Required(ErrorMessage = "Agent ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Agent ID must be greater than 0")]
    public int AgentId { get; set; } 
}