using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    Task MarkAsReadAsync(int id, string userId);
    Task MarkAllAsReadAsync(string userId);
    Task DeleteAsync(int id, string userId);
    Task SendToAllAsync(string message, string type);
}
