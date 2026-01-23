using TicketManager.DTOs.Ticket;
using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;

namespace TicketManager.Services.Interfaces;

public interface ITicketService
{
    Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, int createdByUserId);

    Task<IEnumerable<TicketResponseDto>> GetAllAsync(int userId, Role userRole);

    Task<TicketResponseDto?> GetByIdAsync(int ticketId, int userId, Role userRole);
    Task<IEnumerable<TicketResponseDto>> GetUnassignedAsync();

    Task<bool> AssignAgentAsync(AssignTicketDto dto, int requestingUserId, Role userRole);

    Task<bool> UpdateStatusAsync(int ticketId, UpdateTicketStatusDto dto, int userId, Role userRole);
}