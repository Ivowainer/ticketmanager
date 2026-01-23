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
        var userId = await GetCurrentUserIdAsync();
        if (userId == 0) return Unauthorized();

        var ticket = await _ticketService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetAll()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == 0) return Unauthorized();
        
        return Ok(await _ticketService.GetAllAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketResponseDto>> GetById(int id)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == 0) return Unauthorized();
        
        var ticket = await _ticketService.GetByIdAsync(id, userId);
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
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTicketStatusDto dto)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == 0) return Unauthorized();

        var result = await _ticketService.UpdateStatusAsync(id, dto, userId);
        if (!result)
            return BadRequest(new { message = "Couldn't update the ticket (check permissions or id)" });
        
        return NoContent();
    }
    
    private async Task<int> GetCurrentUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(User); // <-- claim in ControllBase actualUser requesting
        return user?.Id ?? 0;
    }
    
}