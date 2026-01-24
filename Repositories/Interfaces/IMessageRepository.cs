using TicketManager.Models;

namespace TicketManager.Repositories.Interfaces;

public interface IMessageRepository
{
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetByTicketIdAsync(int ticketId);
}