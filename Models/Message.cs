namespace TicketManager.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
    
    public int SenderUserId { get; set; }
    public User SenderUser { get; set; } = null!;
}