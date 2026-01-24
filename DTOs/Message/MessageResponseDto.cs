namespace TicketManager.DTOs.Message;

public class MessageResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;    
    
    public int SenderUserId { get; set; }
    public string SenderUserName { get; set; } = null!;
}