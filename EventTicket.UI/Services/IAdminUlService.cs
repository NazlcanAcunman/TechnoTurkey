using EventTicket.Core.DTOs;
using EventTicket.UI.ViewModels;
using EventTicket.UI.ViewModels.Admin;

namespace EventTicket.UI.Services;

public interface IAdminUlService
{
    string? LastError { get; }
    Task<StatsViewModel> GetStatsAsync();
    Task<PendingViewModel> GetPendingAsync();
    Task ApproveEventAsync(int id);
    Task ApproveVenueAsync(int id);
    Task ApproveArtistAsync(int id);

    // Listeleme
    Task<List<EventCardViewModel>> GetAllEventsAsync();
    Task<List<VenueCardViewModel>> GetAllVenuesAsync();
    Task<List<ArtistCardViewModel>> GetAllArtistsAsync();

    // Silme
    Task DeleteEventAsync(int id);
    Task DeleteVenueAsync(int id);
    Task DeleteArtistAsync(int id);

    // Etkinlik Oluşturma / Düzenleme
    Task CreateEventAsync(CreateEventDto dto);
    Task UpdateEventAsync(int id, UpdateEventDto dto);
    Task<EventCardViewModel?> GetEventByIdAsync(int id);

    // Sanatçı Oluşturma / Düzenleme
    Task CreateArtistAsync(CreateArtistDto dto);
    Task UpdateArtistAsync(int id, UpdateArtistDto dto);
    Task<ArtistCardViewModel?> GetArtistByIdAsync(int id);

    // Mekan Oluşturma / Düzenleme
    Task CreateVenueAsync(CreateVenueDto dto);
    Task UpdateVenueAsync(int id, UpdateVenueDto dto);
    Task<VenueCardViewModel?> GetVenueByIdAsync(int id);
    Task<List<UserViewModel>> GetAllUsersAsync();
    Task DeleteUserAsync(string id);
    Task AssignAdminAsync(string id);
    Task AssignMemberAsync(string id);
}