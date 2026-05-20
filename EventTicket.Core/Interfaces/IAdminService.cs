using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IAdminService
{
    Task<AdminStatsDto> GetStatsAsync();
    Task<PendingItemsDto> GetAllPendingAsync();
    Task<IEnumerable<ContactFormResponseDto>> GetAllMessagesAsync();
    Task SendMessageAsync(ContactFormDto dto);
    Task MarkAsReadAsync(int id);
    Task DeleteMessageAsync(int id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task ToggleUserActiveAsync(string userId);
    Task DeleteUserAsync(string userId);
}