using Microsoft.AspNetCore.SignalR;

namespace TicketManager.Hubs;

public class TicketHub : Hub<ITicketClients>
{
    public async Task JoinTicketGroup(int ticketId)
    {
        var groupName = $"Ticket-{ticketId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveTicketGroup(int ticketId)
    {
        var groupName = $"Ticket-{ticketId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    } 
}