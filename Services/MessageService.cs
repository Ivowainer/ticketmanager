using Microsoft.AspNetCore.Identity;
using TicketManager.DTOs.Message;
using TicketManager.Models;
using TicketManager.Repositories.Interfaces;
using TicketManager.Services.Interfaces;

namespace TicketManager.Services;

public class MessageService(IMessageRepository messageRepository, ITicketRepository ticketRepository, UserManager<User> userManager) : IMessageService
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    
    public async Task<MessageResponseDto?> CreateAsync(int ticketId, CreateMessageDto dto, int senderId, string senderRole)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) 
            throw new KeyNotFoundException("Ticket not found");
        if (ticket.Status == TicketStatus.Closed) return null;
        
        var sender = await userManager.FindByIdAsync(senderId.ToString());
        if (sender == null) return null;
        
        var message = new Message
        {
            Content = dto.Content,
            TicketId = ticketId,
            SenderUserId = senderId,
            SentAt = DateTime.UtcNow
        };
        
        var createdMessage = await messageRepository.CreateAsync(message);

        return MapToDto(createdMessage, sender.Name);

    }

    public async Task<IEnumerable<MessageResponseDto>> GetByTicketIdAsync(int ticketId, int userId, string userRole)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId);
        
        if (ticket == null || !HasAccess(ticket, userId, userRole))
            return [];

        var messages = await messageRepository.GetByTicketIdAsync(ticketId);
        
        return messages.Select(m => MapToDto(m, m.SenderUser?.Name));
    }
    
    private static bool HasAccess(Ticket ticket, int userId, string role)
    {
        if (role == "Admin") return true;
        if (role == "Agent" && ticket.AssignedToUserId == userId) return true;
        if (ticket.CreatedByUserId == userId) return true;
        return false;
    }

    private static MessageResponseDto MapToDto(Message m, string? senderName)
    {
        return new MessageResponseDto
        {
            Id = m.Id,
            Content = m.Content,
            SentAt = m.SentAt,
            SenderUserId = m.SenderUserId,
            SenderUserName = senderName ?? "Unknown"
        };
    }
}