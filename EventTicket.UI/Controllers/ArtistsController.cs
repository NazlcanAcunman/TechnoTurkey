using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class ArtistsController : BaseController
{
    private readonly IArtistUlService _artistService;
    private readonly IProfileUlService _profileService;

    public ArtistsController(IArtistUlService artistService, IProfileUlService profileService)
        : base(profileService)
    {
        _artistService = artistService;
        _profileService = profileService;
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
                // artistId → favoriteId eşlemesi (unfollow için favId gerekli)
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
}