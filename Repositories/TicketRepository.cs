using Microsoft.EntityFrameworkCore;
using TicketManager.Data;
using TicketManager.Models;
using TicketManager.Repositories.Interfaces;

namespace TicketManager.Repositories;

public class TicketRepository(TicketManagerDbContext context) : ITicketRepository
{
    private readonly TicketManagerDbContext _context = context;
    
    public async Task<Ticket> CreateAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }
    
    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByUserIdAsync(int userId)
    {
        return await _context.Tickets
            .Where(t => t.CreatedByUserId == userId)
            .Include(t => t.AssignedToUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(int assignedUserId)
    {
        return await _context.Tickets
            .Where(t => t.AssignedToUserId == assignedUserId)
            .Include(t => t.CreatedByUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetUnassignedAsync()
    {
        return await _context.Tickets
            .Where(t => t.AssignedToUserId == null)
            .Include(t => t.CreatedByUser)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(int ticketId)
    {
        return await _context.Tickets
            .Where(t => t.Id == ticketId)
            .Include(t => t.AssignedToUser)
            .Include(t => t.CreatedByUser)
            .FirstOrDefaultAsync();
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<bool> AssignAgentAsync(int ticketId, int agentId)
    {
        var rowsAffected = await _context.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.AssignedToUserId, agentId)
                .SetProperty(t => t.UpdatedAt, DateTime.UtcNow));

        return rowsAffected > 0;
    }

    public async Task<bool> UpdateStatusAsync(int ticketId, TicketStatus status)
    {
        var rowsAffected = await context.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Status, status)
                .SetProperty(t => t.UpdatedAt, DateTime.UtcNow));

        return rowsAffected > 0;
    }

    public async Task<bool> ExistsAsync(int ticketId)
    {
        return await _context.Tickets.AnyAsync(t => t.Id == ticketId);
    }

    public async Task<bool> IsCreatedByUserAsync(int ticketId, int userId)
    {
        return await _context.Tickets
            .AnyAsync(t => t.Id == ticketId && t.CreatedByUserId == userId);
    }

    public async Task<bool> IsAssignedToUserAsync(int ticketId, int userId)
    {
        return await _context.Tickets
            .AnyAsync(t => t.Id == ticketId && t.AssignedToUserId == userId);
    }
}