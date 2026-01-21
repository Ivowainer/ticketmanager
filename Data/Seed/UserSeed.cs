using TicketManager.Models;

namespace TicketManager.Data.Seed;

public static class UserSeed
{
    public static async Task SeedAsync(TicketManagerDbContext context)
    {
        if (context.Users.Any())
            return;

        Role adminRole = context.Roles.First(r => r.Name == "Admin");
        Role agentRole = context.Roles.First(r => r.Name == "Agent");
        Role customerRole = context.Roles.First(r => r.Name == "Customer");

        var users = new List<User>
        {
            new User
            {
                Name = "Admin",
                Lastname = "Test1",
                RoleId = adminRole.Id
            },
            new User
            {
                Name = "Agent",
                Lastname = "Test2",
                RoleId = agentRole.Id
            },
            new User
            {
                Name = "Customer",
                Lastname = "Test3",
                RoleId = customerRole.Id
            }
        };
        
        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }
}