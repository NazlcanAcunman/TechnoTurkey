using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class ProfileUlService : IProfileUlService
{
    private readonly IApiService _api;

    public ProfileUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<FavoriteViewModel>> GetFavoritesAsync()
        => await _api.GetAsync<List<FavoriteViewModel>>("api/favorites")
           ?? new List<FavoriteViewModel>();

    public async Task<List<NotificationViewModel>> GetNotificationsAsync()
        => await _api.GetAsync<List<NotificationViewModel>>("api/notifications")
           ?? new List<NotificationViewModel>();

    public async Task RemoveFavoriteAsync(int id)
        => await _api.DeleteAsync($"api/favorites/{id}");

    public async Task AddFavoriteAsync(int? venueId, int? artistId)
        => await _api.PostAsync<object>("api/favorites", new { venueId, artistId });

    public async Task MarkNotificationReadAsync(int id)
        => await _api.PatchAsync<object>($"api/notifications/{id}/read");
}