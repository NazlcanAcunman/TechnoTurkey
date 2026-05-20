using EventTicket.Core.DTOs;

namespace EventTicket.UI.Services;

public interface INotificationUlService
{
    Task<List<NotificationResponseDto>> GetMyNotificationsAsync();
    Task<int> GetUnreadCountAsync();
    Task MarkAsReadAsync(int id);
    Task MarkAllAsReadAsync();
    Task DeleteAsync(int id);
    Task SendToAllAsync(string message, string type);
}
