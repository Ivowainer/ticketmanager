namespace TicketManager.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public List<Message> Messages { get; set; } = new List<Message>();
    
    public int? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
}

public enum TicketStatus
{
    Open = 1,
    InProgress = 2,
    Resolved = 3,
    Closed = 4
}