using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IGuestlistUiService
{
    Task<GuestlistRequestViewModel?> CreateAsync(GuestlistApplyViewModel model);
    Task<List<GuestlistRequestViewModel>> GetMyAsync();
    Task<List<GuestlistRequestViewModel>> GetByEventAsync(int eventId);
    Task<GuestlistStatsViewModel?> GetStatsAsync(int eventId);
    Task<bool> UpdateStatusAsync(int id, int status, string? adminNote);
    Task DeleteAsync(int id);
}
