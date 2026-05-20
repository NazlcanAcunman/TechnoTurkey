using EventTicket.UI.ViewModels;

namespace EventTicket.UI.ViewModels.Admin;

public class PendingViewModel
{
    public List<EventCardViewModel> Events { get; set; } = new();
    public List<VenueCardViewModel> Venues { get; set; } = new();
    public List<ArtistCardViewModel> Artists { get; set; } = new();
}
