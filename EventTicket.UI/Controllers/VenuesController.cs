using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class VenuesController : BaseController
{
    private readonly IVenueUlService _venueService;
    private readonly ICommentUlService _commentService;

    public VenuesController(
        IVenueUlService venueService,
        ICommentUlService commentService,
        IProfileUlService profileService)
        : base(profileService)
    {
        _venueService = venueService;
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Mekanlar";
        var venues = await _venueService.GetApprovedVenuesAsync();
        return View(venues);
    }

    public async Task<IActionResult> Details(int id)
    {
        var venue = await _venueService.GetVenueByIdAsync(id);
        if (venue == null) return NotFound();

        var comments = await _commentService.GetByVenueAsync(id);

        var model = new VenueDetailViewModel
        {
            Venue = venue,
            Comments = comments,
            NewComment = new CreateCommentViewModel { VenueId = id }
        };

        ViewData["Title"] = venue.Name;
        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddComment(CreateCommentViewModel newComment)
    {
        if (!string.IsNullOrWhiteSpace(newComment.Content) && newComment.Rating >= 1 && newComment.Rating <= 5)
            await _commentService.AddAsync(newComment);

        return RedirectToAction("Details", new { id = newComment.VenueId });
    }
}