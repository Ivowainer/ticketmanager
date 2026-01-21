using TicketManager.Models;

namespace TicketManager.Repositories.Interfaces;

public interface ITicketRepository
{
       Task AddAsync(Ticket ticket);
       Task<IEnumerable<Ticket>> GetByUserIdAsync(int userId);
       Task<Ticket?> GetByIdAsync(Ticket ticket);
}