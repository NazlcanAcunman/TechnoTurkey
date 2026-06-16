using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class GuestlistController : Controller
{
    private readonly IGuestlistUiService _guestlistService;
    private readonly IEventUlService _eventService;

    public GuestlistController(IGuestlistUiService guestlistService, IEventUlService eventService)
    {
        _guestlistService = guestlistService;
        _eventService = eventService;
    }

    public async Task<IActionResult> Index(int? eventId, int? status)
    {
        ViewData["Title"] = "Guestlist Yönetimi";

        var events = await _eventService.GetApprovedEventsAsync();
        ViewBag.Events = events;
        ViewBag.SelectedEventId = eventId;
        ViewBag.SelectedStatus = status;

        if (eventId.HasValue)
        {
            var requests = await _guestlistService.GetByEventAsync(eventId.Value);
            var stats = await _guestlistService.GetStatsAsync(eventId.Value);

            if (status.HasValue)
                requests = requests.Where(r => r.Status == status.Value).ToList();

            ViewBag.Stats = stats;
            return View(requests);
        }

        return View(new List<EventTicket.UI.ViewModels.GuestlistRequestViewModel>());
    }

    public async Task<IActionResult> Detail(int eventId)
    {
        ViewData["Title"] = "Etkinlik Guestlist";

        var ev = await _eventService.GetEventByIdAsync(eventId);
        if (ev == null) return NotFound();

        var requests = await _guestlistService.GetByEventAsync(eventId);
        var stats = await _guestlistService.GetStatsAsync(eventId);

        ViewBag.Event = ev;
        ViewBag.Stats = stats;
        return View(requests);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id, int eventId)
    {
        var success = await _guestlistService.UpdateStatusAsync(id, 1, null);
        TempData[success ? "Success" : "ApiError"] = success
            ? "Başvuru onaylandı."
            : "Başvuru onaylanırken hata oluştu.";

        return RedirectToAction(nameof(Detail), new { eventId });
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id, int eventId, string? adminNote)
    {
        var success = await _guestlistService.UpdateStatusAsync(id, 2, adminNote);
        TempData[success ? "Success" : "ApiError"] = success
            ? "Başvuru reddedildi."
            : "Başvuru reddedilirken hata oluştu.";

        return RedirectToAction(nameof(Detail), new { eventId });
    }

    public async Task<IActionResult> Export(int eventId)
    {
        var ev = await _eventService.GetEventByIdAsync(eventId);
        var requests = await _guestlistService.GetByEventAsync(eventId);
        var approved = requests.Where(r => r.Status == 1).ToList();

        var sb = new StringBuilder();
        sb.AppendLine("Ad Soyad,E-posta,Telefon,Tür,Başvuru Tarihi");

        foreach (var r in approved)
        {
            var name = $"\"{r.FullName.Replace("\"", "\"\"")}\"";
            var email = $"\"{r.Email}\"";
            var phone = $"\"{r.Phone}\"";
            var type = $"\"{r.GuestlistTypeLabel}\"";
            var date = r.CreatedAt.ToString("dd.MM.yyyy HH:mm");
            sb.AppendLine($"{name},{email},{phone},{type},{date}");
        }

        var fileName = $"guestlist_{ev?.Title?.Replace(" ", "_") ?? eventId.ToString()}_{DateTime.Now:yyyyMMdd}.csv";
        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();

        return File(bytes, "text/csv; charset=utf-8", fileName);
    }
}
