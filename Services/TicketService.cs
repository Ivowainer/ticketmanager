using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;
using TicketManager.Repositories.Interfaces;
using TicketManager.Services.Interfaces;

namespace TicketManager.Services;

public class TicketService(ITicketRepository ticketRepository) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    public async Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, int createdByUserId)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = TicketStatus.Open,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdTicket = await _ticketRepository.CreateAsync(ticket);
        return MapToResponseDto(createdTicket);
    }

    public async Task<IEnumerable<TicketResponseDto>> GetAllAsync(int userId, Role userRole)
    {
        IEnumerable<Ticket> tickets = userRole.Name switch
        {
            "Admin" => await _ticketRepository.GetAllAsync(),
            "Agent" => await _ticketRepository.GetByAssignedUserIdAsync(userId),
            "Customer" => await _ticketRepository.GetByUserIdAsync(userId),
            _ => Enumerable.Empty<Ticket>()
        };

        return tickets.Select(MapToResponseDto);
    }

    public async Task<TicketResponseDto?> GetByIdAsync(int ticketId, int userId, Role userRole)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return null;

        if (userRole.Name == "Admin"
            || (userRole.Name == "Agent" && ticket.AssignedToUserId == userId)
            || (userRole.Name == "Customer" && ticket.CreatedByUserId == userId))
        {
            return MapToResponseDto(ticket);
        }

        return null;
    }

    public async Task<IEnumerable<TicketResponseDto>> GetUnassignedAsync()
    {
        var tickets = await _ticketRepository.GetUnassignedAsync();
        return tickets.Select(MapToResponseDto);
    }

    public Task<bool> AssignAgentAsync(AssignTicketDto dto, int requestingUserId, Role userRole)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateStatusAsync(UpdateTicketStatusDto dto, int userId, Role userRole)
    {
        var ticket = await _ticketRepository.GetByIdAsync(dto.Id);
        if (ticket == null) return false;

        if (userRole.Name != "Admin" &&
            (userRole.Name != "Agent" || ticket.AssignedToUserId != userId))
        {
            return false;
        }

        return await _ticketRepository.UpdateStatusAsync(dto.Id, dto.Status);
    }
    
    private static TicketResponseDto MapToResponseDto(Ticket ticket)
    {
        return new TicketResponseDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedByUserId = ticket.CreatedByUserId,
            CreatedByUserName = ticket.CreatedByUser?.Name!,
            AssignedToUserId = ticket.AssignedToUserId,
            AssignedToUserName = ticket.AssignedToUser?.Name,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }
}