using TicketManager.Models;

namespace TicketManager.DTOs.Ticket.Response;

public class TicketWithMessagesResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = null!;
    
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; } = null!;


}