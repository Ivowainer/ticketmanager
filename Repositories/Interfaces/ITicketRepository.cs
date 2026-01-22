using TicketManager.Models;

namespace TicketManager.Repositories.Interfaces;

public interface ITicketRepository
{
       // Create
       Task<Ticket> CreateAsync(Ticket ticket);
       
       // Get
       Task<IEnumerable<Ticket>> GetAllAsync();
       Task<IEnumerable<Ticket>> GetByUserIdAsync(int userId);
       Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(int assignedUserId);
       Task<IEnumerable<Ticket>> GetUnassignedAsync();
       Task<Ticket?> GetByIdAsync(int ticketId);
       
       // Put
       Task<Ticket> UpdateAsync(Ticket ticket);
       
       // Assign Agent
       Task<bool> AssignAgentAsync(int ticketId, int agentId);
       
       // Change Status
       Task<bool> UpdateStatusAsync(int ticketId, TicketStatus status);
       
       // Validation
       Task<bool> ExistsAsync(int ticketId);
       Task<bool> IsCreatedByUserAsync(int ticketId, int userId);
       Task<bool> IsAssignedToUserAsync(int ticketId, int userId);
}