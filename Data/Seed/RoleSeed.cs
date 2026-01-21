using TicketManager.Models;

namespace TicketManager.Data.Seed;

public static class RoleSeed
{
    public static async Task SeedAsync(TicketManagerDbContext context)
    {
        if (context.Roles.Any())
            return;

        var roles = new List<Role>
        {
            new Role { Name = "Admin" },
            new Role { Name = "Agent" },
            new Role { Name = "Customer" }
        };
        
        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();
    } 
}