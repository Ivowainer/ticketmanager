using TicketManager.Data;
using TicketManager.Models;
using TicketManager.Repositories.Interfaces;

namespace TicketManager.Repositories;

public class TicketRepository(TicketManagerDbContext context) : ITicketRepository
{
    private readonly TicketManagerDbContext _context = context;
    
    public Task CreateAsync(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Ticket>> GetByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> GetByAssignedUserIdAsync(int assignedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> GetUnassignedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Ticket?> GetByIdAsync(int ticketId)
    {
        throw new NotImplementedException();
    }

    public Task<Ticket> UpdateAsync(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AssignAgentAsync(int ticketId, int agentId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateStatusAsync(int ticketId, TicketStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int ticketId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsCreatedByUserAsync(int ticketId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAssignedToUserAsync(int ticketId, int userId)
    {
        throw new NotImplementedException();
    }
}