using TicketManager.DTOs.Ticket;
using TicketManager.Models;

namespace TicketManager.Services.Interfaces;

public interface ITicketService
{
    Task<Ticket> CreateTicketAsync(int userId, CreateTicketDto createTicketDto);
    Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId);
    Task<Ticket> GetTicketByIdAsync(int userId, int ticketId);
}