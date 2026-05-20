using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketResponseDto>> PurchaseAsync(PurchaseTicketDto dto, string userId);
    Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(string userId);
    Task<TicketResponseDto?> GetByIdAsync(int id);
}