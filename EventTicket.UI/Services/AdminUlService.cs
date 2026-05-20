using EventTicket.Core.DTOs;
using EventTicket.UI.ViewModels;
using EventTicket.UI.ViewModels.Admin;

namespace EventTicket.UI.Services;

public class AdminUlService : IAdminUlService
{
    private readonly IApiService _api;

    public AdminUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<StatsViewModel> GetStatsAsync()
        => await _api.GetAsync<StatsViewModel>("api/admin/stats") ?? new StatsViewModel();

    public async Task<PendingViewModel> GetPendingAsync()
        => await _api.GetAsync<PendingViewModel>("api/admin/pending") ?? new PendingViewModel();

    public async Task ApproveEventAsync(int id)
        => await _api.PatchAsync<object>($"api/events/{id}/approve");

    public async Task ApproveVenueAsync(int id)
        => await _api.PatchAsync<object>($"api/venues/{id}/approve");

    public async Task ApproveArtistAsync(int id)
        => await _api.PatchAsync<object>($"api/artists/{id}/approve");

    public async Task<List<EventCardViewModel>> GetAllEventsAsync()
        => await _api.GetAsync<List<EventCardViewModel>>("api/events/all") ?? new();

    public async Task<List<VenueCardViewModel>> GetAllVenuesAsync()
        => await _api.GetAsync<List<VenueCardViewModel>>("api/venues") ?? new();

    public async Task<List<ArtistCardViewModel>> GetAllArtistsAsync()
        => await _api.GetAsync<List<ArtistCardViewModel>>("api/artists") ?? new();

    public async Task DeleteEventAsync(int id)
        => await _api.DeleteAsync($"api/events/{id}");

    public async Task DeleteVenueAsync(int id)
        => await _api.DeleteAsync($"api/venues/{id}");

    public async Task DeleteArtistAsync(int id)
        => await _api.DeleteAsync($"api/artists/{id}");

    public async Task CreateEventAsync(CreateEventDto dto)
    {
        var result = await _api.PostAsync<object>("api/events", dto);
        if (result == null && _api.LastError != null)
            throw new Exception(_api.LastError);
    }

    public async Task UpdateEventAsync(int id, UpdateEventDto dto)
        => await _api.PutAsync($"api/events/{id}", dto);

    public async Task<EventCardViewModel?> GetEventByIdAsync(int id)
        => await _api.GetAsync<EventCardViewModel>($"api/events/{id}");

    public async Task<List<UserViewModel>> GetAllUsersAsync()
        => await _api.GetAsync<List<UserViewModel>>("api/admin/users") ?? new();

    public async Task DeleteUserAsync(string id)
        => await _api.DeleteAsync($"api/admin/users/{id}");

    public async Task AssignAdminAsync(string id)
        => await _api.PatchAsync<object>($"api/admin/users/{id}/assign-admin");

    public async Task AssignMemberAsync(string id)
        => await _api.PatchAsync<object>($"api/admin/users/{id}/assign-member");
}