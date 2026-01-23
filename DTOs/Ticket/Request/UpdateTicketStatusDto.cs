using System.ComponentModel.DataAnnotations;
using TicketManager.Models;

namespace TicketManager.DTOs.Ticket.Request;

public class UpdateTicketStatusDto
{
    [Required(ErrorMessage = "Ticket ID is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Status is required")]
    public TicketStatus Status { get; set; }
}