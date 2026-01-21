namespace TicketManager.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
}