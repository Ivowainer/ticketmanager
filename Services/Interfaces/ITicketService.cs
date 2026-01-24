using TicketManager.DTOs.Ticket;
using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;

namespace TicketManager.Services.Interfaces;

public interface ITicketService
{
    Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, int createdByUserId);

    Task<IEnumerable<TicketResponseDto>> GetAllAsync(int userId, string userRole);

    Task<TicketResponseDto?> GetByIdAsync(int ticketId, int userId, string userRole);
    Task<IEnumerable<TicketResponseDto>> GetUnassignedAsync();

    Task<bool> AssignAgentAsync(int ticketId, int agentId, int requestingUserId, string requestingUserRole);

    Task<bool> UpdateStatusAsync(int ticketId, TicketStatus status, int userId, string userRole);
}