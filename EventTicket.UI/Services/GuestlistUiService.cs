using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class GuestlistUiService : IGuestlistUiService
{
    private readonly IApiService _api;

    public GuestlistUiService(IApiService api)
    {
        _api = api;
    }

    public async Task<GuestlistRequestViewModel?> CreateAsync(GuestlistApplyViewModel model)
    {
        return await _api.PostAsync<GuestlistRequestViewModel>("api/guestlist", new
        {
            eventId       = model.EventId,
            guestName     = model.GuestName,
            guestPhone    = model.GuestPhone,
            note          = model.Note,
            gender        = model.Gender,
            termsAccepted = model.TermsAccepted
        });
    }

    public async Task<List<GuestlistRequestViewModel>> GetMyAsync()
    {
        return await _api.GetAsync<List<GuestlistRequestViewModel>>("api/guestlist/my")
               ?? new List<GuestlistRequestViewModel>();
    }

    public async Task<List<GuestlistRequestViewModel>> GetByEventAsync(int eventId)
    {
        return await _api.GetAsync<List<GuestlistRequestViewModel>>($"api/guestlist/event/{eventId}")
               ?? new List<GuestlistRequestViewModel>();
    }

    public async Task<GuestlistStatsViewModel?> GetStatsAsync(int eventId)
    {
        return await _api.GetAsync<GuestlistStatsViewModel>($"api/guestlist/event/{eventId}/stats");
    }

    public async Task<bool> UpdateStatusAsync(int id, int status, string? adminNote)
    {
        var result = await _api.PutAsync<GuestlistRequestViewModel>($"api/guestlist/{id}/status", new
        {
            status,
            adminNote
        });
        return result != null;
    }

    public async Task DeleteAsync(int id)
    {
        await _api.DeleteAsync($"api/guestlist/{id}");
    }
}
