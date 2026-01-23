using TicketManager.DTOs.Ticket;
using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;

namespace TicketManager.Services.Interfaces;

public interface ITicketService
{
    Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, int createdByUserId);

    Task<IEnumerable<TicketResponseDto>> GetAllAsync(int userId);

    Task<TicketResponseDto?> GetByIdAsync(int ticketId, int userId);
    Task<IEnumerable<TicketResponseDto>> GetUnassignedAsync();

    Task<bool> AssignAgentAsync(int ticketId, int agentId, int requestingUserId);

    Task<bool> UpdateStatusAsync(int ticketId, UpdateTicketStatusDto dto, int userId);
}