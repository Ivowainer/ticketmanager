using TicketManager.DTOs.Message;

namespace TicketManager.Services.Interfaces;

public interface IMessageService
{
    Task<MessageResponseDto?> CreateAsync(int ticketId, CreateMessageDto dto, int senderId, string senderRole);
    Task<IEnumerable<MessageResponseDto>> GetByTicketIdAsync(int ticketId, int userId, string userRole);
}