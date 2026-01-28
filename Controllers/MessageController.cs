using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TicketManager.DTOs.Message;
using TicketManager.Hubs;
using TicketManager.Services.Interfaces;

namespace TicketManager.Controllers;

[ApiController]
[Route("api/ticket/{ticketId}/message")]
[Authorize]
public class MessageController(IMessageService messageService, IHubContext<TicketHub, ITicketClients> hubContext) : ControllerBase
{
    private readonly IMessageService _messageService = messageService;
    private readonly IHubContext<TicketHub, ITicketClients> _hubContext = hubContext;
    
    // INTERNAL FUNC.
    private int GetCurrentUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return (idClaim != null && int.TryParse(idClaim.Value, out int userId)) ? userId : 0;
    }

    private string GetCurrentUserRole() => User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    
    // Endpointss
    [HttpGet]
    public async Task<IActionResult> GetAll(int ticketId)
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();
        if (userId == 0) return Unauthorized();

        var messages = await _messageService.GetByTicketIdAsync(ticketId, userId, role);
        return Ok(messages);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(int ticketId, [FromBody] CreateMessageDto dto)
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();
        if (userId == 0) return Unauthorized();

        var messageDto = await messageService.CreateAsync(ticketId, dto, userId, role);

        if (messageDto == null)
            return BadRequest(new { message = "The message couldn't be sent" });

        await hubContext.Clients.Group($"Ticket-{ticketId}").ReceiveMessage(messageDto);

        return CreatedAtAction(nameof(GetAll), new { ticketId }, messageDto);
    }
}