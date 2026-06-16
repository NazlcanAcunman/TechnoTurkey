using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class ArtistsController : BaseController
{
    private readonly IArtistUlService _artistService;
    private readonly IProfileUlService _profileService;
    private readonly IEventUlService _eventService;

    public ArtistsController(IArtistUlService artistService, IProfileUlService profileService, IEventUlService eventService)
        : base(profileService)
    {
        _artistService = artistService;
        _profileService = profileService;
        _eventService = eventService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Sanatçılar";
        var artists = await _artistService.GetApprovedArtistsAsync();

        if (User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var favorites = await _profileService.GetFavoritesAsync();
                ViewBag.FollowedArtistMap = favorites
                    .Where(f => f.ArtistId != null && f.ArtistId > 0)
                    .ToDictionary(f => (int)f.ArtistId!, f => f.Id);
            }
            catch { ViewBag.FollowedArtistMap = new Dictionary<int, int>(); }
        }
        else
        {
            ViewBag.FollowedArtistMap = new Dictionary<int, int>();
        }

        return View(artists);
    }

    public async Task<IActionResult> Details(int id)
    {
        var artist = await _artistService.GetArtistByIdAsync(id);
        if (artist == null) return NotFound();

        var events = await _eventService.GetFilteredEventsAsync(artistName: artist.Name);

        if (User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var favorites = await _profileService.GetFavoritesAsync();
                ViewBag.FollowedArtistMap = favorites
                    .Where(f => f.ArtistId != null && f.ArtistId > 0)
                    .ToDictionary(f => (int)f.ArtistId!, f => f.Id);
            }
            catch { ViewBag.FollowedArtistMap = new Dictionary<int, int>(); }
        }
        else
        {
            ViewBag.FollowedArtistMap = new Dictionary<int, int>();
        }

        ViewBag.ArtistEvents = events;
        ViewData["Title"] = artist.Name;
        return View(artist);
    }
}
