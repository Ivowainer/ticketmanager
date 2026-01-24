using Microsoft.AspNetCore.Identity;
using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;
using TicketManager.Repositories.Interfaces;
using TicketManager.Services.Interfaces;

namespace TicketManager.Services;

public class TicketService(ITicketRepository ticketRepository, UserManager<User> userManager) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    private readonly UserManager<User> _userManager = userManager;
    
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

    public async Task<IEnumerable<TicketResponseDto>> GetAllAsync(int userId, string userRole)
    {  
        IEnumerable<Ticket> tickets = userRole switch
        {
            "Admin" => await _ticketRepository.GetAllAsync(), // admin
            "Agent" => await _ticketRepository.GetByAssignedUserIdAsync(userId), // agent
            _       => await _ticketRepository.GetByUserIdAsync(userId) // ccustomer
        };

        return tickets.Select(MapToResponseDto);
    }

    public async Task<TicketResponseDto?> GetByIdAsync(int ticketId, int userId, string userRole)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return null;

        bool isAdmin = userRole == "Admin";
        bool isAgent = userRole == "Agent";
        bool isCreator = ticket.CreatedByUserId == userId;
        bool isAssigned = ticket.AssignedToUserId == userId;
        
        // admin can see everything - agent can only see the assigned tickets - customer can only view yours  
        if (isAdmin || (isAgent && isAssigned) || isCreator)
            return MapToResponseDto(ticket);
        
        return null;
    }

    public async Task<IEnumerable<TicketResponseDto>> GetUnassignedAsync()
    {
        var tickets = await _ticketRepository.GetUnassignedAsync();
        return tickets.Select(MapToResponseDto);
    }

    public async Task<bool> AssignAgentAsync(int ticketId, int agentId, int requestingUserId, string requestingUserRole)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        if (ticket.Status == TicketStatus.Closed) return false;

        bool isAdmin = requestingUserRole == "Admin";
        bool isAgent = requestingUserRole == "Agent"; 
        if (!isAdmin && !isAgent) return false;

        if (isAgent && agentId != requestingUserId) return false;

        if (agentId != requestingUserId)
        {
            var targetUser = await _userManager.FindByIdAsync(agentId.ToString());
            if (targetUser == null) return false;

            bool targetIsAgent = await _userManager.IsInRoleAsync(targetUser, "Agent");
            bool targetIsAdmin = await _userManager.IsInRoleAsync(targetUser, "Admin");

            if (!targetIsAdmin && !targetIsAgent) return false;
        }

        await _ticketRepository.AssignAgentAsync(ticketId, agentId);
        return true;

    }

    public async Task<bool> UpdateStatusAsync(int ticketId, TicketStatus status, int userId, string userRole)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        bool canUpdate = userRole == "Admin" ||
                         (userRole == "Agent" && ticket.AssignedToUserId == userId);

        if (!canUpdate) return false;
        return await _ticketRepository.UpdateStatusAsync(ticketId, status);
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