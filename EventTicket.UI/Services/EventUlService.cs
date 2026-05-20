using EventTicket.Core.Interfaces;
using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class EventUlService : IEventUlService
{
    private readonly IApiService _api;

    public EventUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<EventCardViewModel>> GetApprovedEventsAsync()
    {
        return await _api.GetAsync<List<EventCardViewModel>>("api/events")
               ?? new List<EventCardViewModel>();
    }

    public async Task<EventCardViewModel?> GetEventByIdAsync(int id)
    {
        return await _api.GetAsync<EventCardViewModel>($"api/events/{id}");
    }

    public async Task<List<EventCardViewModel>> GetFilteredEventsAsync(
        string? search = null,
        string? city = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? artistName = null,
        string sortBy = "date")
    {
        var allEvents = await GetApprovedEventsAsync();

        // Filtreleme işlemleri
        if (!string.IsNullOrWhiteSpace(search))
            allEvents = allEvents
                .Where(e => e.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                         || e.Description.Contains(search, StringComparison.OrdinalIgnoreCase)
                         || (!string.IsNullOrEmpty(e.ArtistName) && e.ArtistName.Contains(search, StringComparison.OrdinalIgnoreCase))
                         || (!string.IsNullOrEmpty(e.ArtistGenre) && e.ArtistGenre.Contains(search, StringComparison.OrdinalIgnoreCase))
                         || (!string.IsNullOrEmpty(e.VenueName) && e.VenueName.Contains(search, StringComparison.OrdinalIgnoreCase)))
                .ToList();

        if (!string.IsNullOrWhiteSpace(city))
            allEvents = allEvents
                .Where(e => e.VenueCity.Contains(city, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (dateFrom.HasValue)
            allEvents = allEvents.Where(e => e.Date >= dateFrom.Value).ToList();

        if (dateTo.HasValue)
            allEvents = allEvents.Where(e => e.Date <= dateTo.Value.AddDays(1)).ToList();

        if (minPrice.HasValue)
            allEvents = allEvents.Where(e => e.TicketPrice >= minPrice.Value).ToList();

        if (maxPrice.HasValue)
            allEvents = allEvents.Where(e => e.TicketPrice <= maxPrice.Value).ToList();

        if (!string.IsNullOrWhiteSpace(artistName))
            allEvents = allEvents
                .Where(e => e.ArtistName.Contains(artistName, StringComparison.OrdinalIgnoreCase))
                .ToList();

        // Sıralama
        allEvents = sortBy switch
        {
            "price_asc" => allEvents.OrderBy(e => e.TicketPrice).ToList(),
            "price_desc" => allEvents.OrderByDescending(e => e.TicketPrice).ToList(),
            "name" => allEvents.OrderBy(e => e.Title).ToList(),
            _ => allEvents.OrderBy(e => e.Date).ToList()  // "date" varsayılan
        };

        return allEvents;
    }
}