using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IProfileUlService
{
    Task<List<FavoriteViewModel>> GetFavoritesAsync();
    Task<List<NotificationViewModel>> GetNotificationsAsync();
    Task RemoveFavoriteAsync(int id);
    Task AddFavoriteAsync(int? venueId, int? artistId);
    Task MarkNotificationReadAsync(int id);
}