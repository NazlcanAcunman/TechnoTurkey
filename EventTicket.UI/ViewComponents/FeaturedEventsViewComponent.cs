using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.ViewComponents;

/// <summary>
/// Öne çıkan / yaklaşan etkinlikleri render eder.
/// Kullanım: @await Component.InvokeAsync("FeaturedEvents", new { count = 6 })
/// </summary>
public class FeaturedEventsViewComponent : ViewComponent
{
    private readonly IEventUlService _eventService;

    public FeaturedEventsViewComponent(IEventUlService eventService)
    {
        _eventService = eventService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count = 6, string? city = null)
    {
        var all = await _eventService.GetApprovedEventsAsync();

        var filtered = all
            .Where(e => e.Date >= DateTime.Now)
            .Where(e => string.IsNullOrEmpty(city) || e.VenueCity == city)
            .OrderBy(e => e.Date)
            .Take(count)
            .ToList();

        return View(filtered);
    }
}
