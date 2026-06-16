using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class EventsController : BaseController
{
    private readonly IEventUlService _eventService;
    private readonly ICommentUlService _commentService;
    private readonly ITicketUlService _ticketService;

    public EventsController(
        IEventUlService eventService,
        ICommentUlService commentService,
        ITicketUlService ticketService,
        IProfileUlService profileService)
        : base(profileService)
    {
        _eventService = eventService;
        _commentService = commentService;
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index(
        string? search,
        string? city,
        DateTime? dateFrom,
        DateTime? dateTo,
        decimal? minPrice,
        decimal? maxPrice,
        string? artistName,
        string sortBy = "date")
    {
        ViewData["Title"] = "Etkinlikler";

        // Şehir filtresini Türkçe karakter normalize ederek karşılaştır
        var normalizedCity = Normalize(city);

        var allEvents = await _eventService.GetApprovedEventsAsync();

        // Normalize edilmiş şehir karşılaştırması
        var matchedCity = string.IsNullOrEmpty(normalizedCity)
            ? city
            : allEvents
                .Select(e => e.VenueCity)
                .Where(v => !string.IsNullOrEmpty(v))
                .FirstOrDefault(v => Normalize(v).Equals(normalizedCity, StringComparison.OrdinalIgnoreCase))
              ?? city;

        var events = await _eventService.GetFilteredEventsAsync(
            search, matchedCity, dateFrom, dateTo, minPrice, maxPrice, artistName, sortBy);

        var model = new EventFilterViewModel
        {
            Events = events,
            Search = search,
            City = city,
            DateFrom = dateFrom,
            DateTo = dateTo,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            ArtistName = artistName,
            SortBy = sortBy,
            AvailableCities = allEvents
                .Select(e => e.VenueCity)
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .OrderBy(v => v)
                .ToList(),
            AvailableArtists = allEvents
                .Select(e => e.ArtistName)
                .Where(a => !string.IsNullOrEmpty(a))
                .Distinct()
                .OrderBy(a => a)
                .ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var ev = await _eventService.GetEventByIdAsync(id);
        if (ev == null) return NotFound();

        var comments = await _commentService.GetByEventAsync(id);

        var model = new EventDetailViewModel
        {
            Event = ev,
            Comments = comments,
            NewComment = new CreateCommentViewModel { EventId = id }
        };

        ViewData["Title"] = ev.Title;
        return View(model);
    }

    public IActionResult DummyDetail(string title, string artist, string venue, string city, string date, string img, string ticketUrl)
    {
        ViewData["Title"] = title;
        ViewBag.EventTitle  = title;
        ViewBag.Artist      = artist;
        ViewBag.Venue       = venue;
        ViewBag.City        = city;
        ViewBag.Date        = date;
        ViewBag.Img         = img;
        ViewBag.TicketUrl   = ticketUrl;
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddComment(CreateCommentViewModel newComment)
    {
        if (!string.IsNullOrWhiteSpace(newComment.Content) && newComment.Rating >= 1 && newComment.Rating <= 5)
            await _commentService.AddAsync(newComment);

        return RedirectToAction("Details", new { id = newComment.EventId });
    }

    private static string Normalize(string? input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        return input
            .Replace('ı', 'i').Replace('İ', 'i')
            .Replace('ğ', 'g').Replace('Ğ', 'g')
            .Replace('ü', 'u').Replace('Ü', 'u')
            .Replace('ş', 's').Replace('Ş', 's')
            .Replace('ö', 'o').Replace('Ö', 'o')
            .Replace('ç', 'c').Replace('Ç', 'c')
            .ToLowerInvariant();
    }
}
