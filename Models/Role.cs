using Microsoft.AspNetCore.Identity;

namespace TicketManager.Models;

public class Role : IdentityRole<int>
{
    public ICollection<User> Users { get; set; } = new List<User>();
}