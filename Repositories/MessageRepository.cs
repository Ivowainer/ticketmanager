using Microsoft.EntityFrameworkCore;
using TicketManager.Data;
using TicketManager.Models;
using TicketManager.Repositories.Interfaces;

namespace TicketManager.Repositories;

public class MessageRepository(TicketManagerDbContext context) : IMessageRepository
{
    private readonly TicketManagerDbContext _context = context;
    
    public async Task<Message> CreateAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<Message>> GetByTicketIdAsync(int ticketId)
    {
        return await _context.Messages
            .Where(m => m.TicketId == ticketId)
            .Include(m => m.SenderUser)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}