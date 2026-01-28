using TicketManager.DTOs.Message;

namespace TicketManager.Hubs;

public interface ITicketClients
{
    Task ReceiveMessage(MessageResponseDto message);
}