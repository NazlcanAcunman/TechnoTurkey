using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

[Authorize]
public class GuestlistController : Controller
{
    private readonly IGuestlistUiService _guestlistService;
    private readonly IEventUlService _eventService;

    public GuestlistController(IGuestlistUiService guestlistService, IEventUlService eventService)
    {
        _guestlistService = guestlistService;
        _eventService = eventService;
    }

    [AllowAnonymous]
    [HttpGet("/Guestlist")]
    public async Task<IActionResult> Index()
    {
        var events = await _eventService.GetApprovedEventsAsync();
        ViewData["Title"] = "Guestlist";
        return View(events);
    }

    [HttpGet("/guestlist/apply/{eventId:int}")]
    public async Task<IActionResult> Apply(int eventId)
    {
        var ev = await _eventService.GetEventByIdAsync(eventId);
        if (ev == null) return NotFound();

        var model = new GuestlistApplyViewModel
        {
            EventId = ev.Id,
            EventTitle = ev.Title,
            EventDate = ev.Date,
            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty
        };

        ViewData["Title"] = "Guestlist Başvurusu";
        return View(model);
    }

    [HttpPost("/guestlist/apply")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(GuestlistApplyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var ev = await _eventService.GetEventByIdAsync(model.EventId);
            model.EventTitle = ev?.Title ?? string.Empty;
            model.EventDate = ev?.Date ?? DateTime.UtcNow;
            ViewData["Title"] = "Guestlist Başvurusu";
            return View(model);
        }

        var result = await _guestlistService.CreateAsync(model);
        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "Başvuru oluşturulamadı. Bu etkinliğe zaten başvurmuş olabilirsiniz.");
            var ev = await _eventService.GetEventByIdAsync(model.EventId);
            model.EventTitle = ev?.Title ?? string.Empty;
            model.EventDate = ev?.Date ?? DateTime.UtcNow;
            ViewData["Title"] = "Guestlist Başvurusu";
            return View(model);
        }

        TempData["Success"] = "Guestlist başvurunuz alındı. Onay durumunu profilinizden takip edebilirsiniz.";
        return RedirectToAction(nameof(My));
    }

    [HttpGet("/guestlist/my")]
    public async Task<IActionResult> My()
    {
        var requests = await _guestlistService.GetMyAsync();
        ViewData["Title"] = "Guestlist Başvurularım";
        return View(requests);
    }
}
