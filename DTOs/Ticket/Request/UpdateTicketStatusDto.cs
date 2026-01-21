using System.ComponentModel.DataAnnotations;
using TicketManager.Models;

namespace TicketManager.DTOs.Ticket.Request;

public class UpdateTicketStatusDto
{
    [Required(ErrorMessage = "Status is required")]
    public TicketStatus Status { get; set; }
}