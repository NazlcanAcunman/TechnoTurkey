using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;


public interface IEventUlService
{
    Task<List<EventCardViewModel>> GetApprovedEventsAsync();
    Task<EventCardViewModel?> GetEventByIdAsync(int id);
    Task<List<EventCardViewModel>> GetFilteredEventsAsync(
        string? search = null,
        string? city = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? artistName = null,
        string sortBy = "date");
}