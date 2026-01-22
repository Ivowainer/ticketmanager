using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;
using TicketManager.Services.Interfaces;

namespace TicketManager.Services;

public class TicketService : ITicketService
{
    public Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, int createdByUserId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketResponseDto>> GetAllAsync(int userId, Role userRole)
    {
        throw new NotImplementedException();
    }

    public Task<TicketResponseDto?> GetByIdAsync(int ticketId, int userId, Role userRole)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketResponseDto>> GetUnassignedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> AssignAgentAsync(AssignTicketDto dto, int requestingUserId, Role userRole)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateStatusAsync(UpdateTicketStatusDto dto, int userId, Role userRole)
    {
        throw new NotImplementedException();
    }
}