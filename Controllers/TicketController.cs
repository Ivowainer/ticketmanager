using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;
using TicketManager.Services.Interfaces;

namespace TicketManager.Controllers;

// TODO: delete double fetchng in repository -> service -> controller
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketController(ITicketService ticketService, UserManager<User> userManager) : ControllerBase
{
    private readonly ITicketService _ticketService = ticketService;
    private readonly UserManager<User> _userManager = userManager;
    
    [HttpPost]
    public async Task<ActionResult<TicketResponseDto>> Create([FromBody] CreateTicketDto dto)
    {
        var userId = GetCurrentUserId();
        if (userId == 0) return Unauthorized();

        var ticket = await _ticketService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetAll()
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole(); 
        if (userId == 0) return Unauthorized();
        
        return Ok(await _ticketService.GetAllAsync(userId, userRole));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketResponseDto>> GetById(int id)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();
        if (userId == 0) return Unauthorized();
        
        var ticket = await _ticketService.GetByIdAsync(id, userId, userRole);
        if (ticket == null)
            return NotFound(new { message = "Ticket not found or access denied" });
        
        return Ok(ticket);
    }

    [HttpGet("unassigned")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetUnassigned()
    {
        var tickets = await _ticketService.GetUnassignedAsync();
        return Ok(tickets);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] TicketStatus status)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();
        
        if (userId == 0) return Unauthorized();

        if (!Enum.IsDefined(typeof(TicketStatus), status))
            return BadRequest(new { message = "Invalid status value" });

        var result = await _ticketService.UpdateStatusAsync(id, status, userId, userRole);
        if (!result)
            return BadRequest(new { message = "Couldn't update the ticket (check permissions or id)" });
        
        return NoContent();
    }
    
    [HttpPatch("{id}/assign")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> AssignTicket(int id, [FromQuery] int agentId)
    {
        var userId = GetCurrentUserId();
        var role = GetCurrentUserRole();

        // If it doesn't send any agent ID or if it's 0, assign it to the same user who requested it
        int targetAgentId = agentId > 0 ? agentId : userId;

        var result = await ticketService.AssignAgentAsync(id, targetAgentId, userId, role);

        if (!result)
            return BadRequest(new { message = "Cannot assign ticket. Check permissions or agent validity." });

        return NoContent();
    }
    
    private int GetCurrentUserId()
    {
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (idClaim != null && int.TryParse(idClaim.Value, out var userId))
            return userId;

        return 0;
    }

    private string GetCurrentUserRole() { return User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty; }
    
}