using EventTicket.Core.DTOs;

namespace EventTicket.UI.Services;

public class NotificationUlService : INotificationUlService
{
    private readonly IApiService _api;

    public NotificationUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<NotificationResponseDto>> GetMyNotificationsAsync()
    {
        return await _api.GetAsync<List<NotificationResponseDto>>("api/notifications")
               ?? new List<NotificationResponseDto>();
    }

    public async Task<int> GetUnreadCountAsync()
    {
        var result = await _api.GetAsync<UnreadCountResponse>("api/notifications/unread-count");
        return result?.Count ?? 0;
    }

    public async Task MarkAsReadAsync(int id)
    {
        await _api.PatchAsync<object>($"api/notifications/{id}/read");
    }

    public async Task MarkAllAsReadAsync()
    {
        await _api.PatchAsync<object>("api/notifications/read-all");
    }

    public async Task DeleteAsync(int id)
    {
        await _api.DeleteAsync($"api/notifications/{id}");
    }

    public async Task SendToAllAsync(string message, string type)
    {
        await _api.PostAsync<object>("api/notifications/send-all", new { message, type });
    }

    private class UnreadCountResponse
    {
        public int Count { get; set; }
    }
}
