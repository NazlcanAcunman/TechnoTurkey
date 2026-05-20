using EventTicket.Core.Interfaces;
using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class ArtistUlService : IArtistUlService
{
    private readonly IApiService _api;

    public ArtistUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<ArtistCardViewModel>> GetApprovedArtistsAsync()
    {
        return await _api.GetAsync<List<ArtistCardViewModel>>("api/artists")
               ?? new List<ArtistCardViewModel>();
    }

    public async Task<ArtistCardViewModel?> GetArtistByIdAsync(int id)
    {
        return await _api.GetAsync<ArtistCardViewModel>($"api/artists/{id}");
    }
}