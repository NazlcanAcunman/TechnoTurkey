using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IArtistUlService
{
    Task<List<ArtistCardViewModel>> GetApprovedArtistsAsync();
    Task<ArtistCardViewModel?> GetArtistByIdAsync(int id);
}