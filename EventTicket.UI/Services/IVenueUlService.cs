using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IVenueUlService
{
    Task<List<VenueCardViewModel>> GetApprovedVenuesAsync();
    Task<VenueCardViewModel?> GetVenueByIdAsync(int id);
}