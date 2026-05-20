using EventTicket.Core.Interfaces;
using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class VenueUlService : IVenueUlService
{
    private readonly IApiService _api;

    public VenueUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<VenueCardViewModel>> GetApprovedVenuesAsync()
    {
        return await _api.GetAsync<List<VenueCardViewModel>>("api/venues")
               ?? new List<VenueCardViewModel>();
    }

    public async Task<VenueCardViewModel?> GetVenueByIdAsync(int id)
    {
        return await _api.GetAsync<VenueCardViewModel>($"api/venues/{id}");
    }
}