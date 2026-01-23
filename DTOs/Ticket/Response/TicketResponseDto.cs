using TicketManager.Models;

namespace TicketManager.DTOs.Ticket.Response;

public class TicketResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public int CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = null!;
    
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
}