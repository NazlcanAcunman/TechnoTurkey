using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventResponseDto>> GetAllApprovedAsync();
    Task<IEnumerable<EventResponseDto>> GetAllPendingAsync();
    Task<IEnumerable<EventResponseDto>> GetAllForAdminAsync();
    Task<IEnumerable<EventResponseDto>> GetByVenueAsync(int venueId);
    Task<EventResponseDto?> GetByIdAsync(int id);
    Task<EventResponseDto?> GetByIdForAdminAsync(int id);
    Task<EventResponseDto> CreateAsync(CreateEventDto dto, string adminUserId, bool isSuperAdmin);
    Task UpdateAsync(int id, UpdateEventDto dto, string adminUserId, bool isSuperAdmin);
    Task DeleteAsync(int id);
    Task HardDeleteAsync(int id);
    Task ApproveAsync(int id);
    Task RejectAsync(int id, string reason);
}
