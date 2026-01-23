using Microsoft.AspNetCore.Mvc;
using TicketManager.DTOs.Ticket.Request;
using TicketManager.DTOs.Ticket.Response;
using TicketManager.Models;
using TicketManager.Services.Interfaces;

namespace TicketManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    private readonly ITicketService _ticketService = ticketService;

    [HttpPost]
    public async Task<ActionResult<TicketResponseDto>> Create([FromBody] CreateTicketDto dto)
    {
        // TODO: Get the user id authenticated
        // HARDCODED
        var userId = 3; // customer

        var ticket = await _ticketService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetAll()
    {
        // TODO: Get the user id authenticated
        // HARDCODED
        var userId = 1; // admin
        var userRole = new Role { Id = 1, Name = "Admin" };
        
        return Ok(await _ticketService.GetAllAsync(userId, userRole));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketResponseDto>> GetById(int id)
    {
        // TODO: Get the user id authenticated
        // Hardcoded
        var userId = 1; // admin
        var userRole = new Role { Id = 1, Name = "Admin" };

        var ticket = await _ticketService.GetByIdAsync(id, userId, userRole);
        if (ticket == null)
            return NotFound(new { message = "Ticket not found o you don't have permissions" });
        
        return Ok(ticket);
    }

    [HttpGet("unassigned")]
    public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetUnassigned()
    {
        var tickets = await _ticketService.GetUnassignedAsync();
        return Ok(tickets);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStats(int id, [FromBody] UpdateTicketStatusDto dto)
    {
        // TODO: Get the user id authenticated
        var userId = 1; // admin
        var userRole = new Role { Id = 1, Name = "Admin" };

        var result = await _ticketService.UpdateStatusAsync(id, dto, userId, userRole);
        if (!result)
            return BadRequest(new { message = "Couldn't update the ticket" });
        
        return NoContent();
    }
    
}