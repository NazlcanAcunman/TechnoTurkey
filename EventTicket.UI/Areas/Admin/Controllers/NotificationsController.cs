using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class NotificationsController : Controller
{
    private readonly INotificationUlService _notificationService;

    public NotificationsController(INotificationUlService notificationService)
    {
        _notificationService = notificationService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendToAll(string message, string type)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            await _notificationService.SendToAllAsync(message, string.IsNullOrWhiteSpace(type) ? "info" : type);
            TempData["Success"] = "Bildirim tüm kullanıcılara gönderildi.";
        }
        return RedirectToAction("Index");
    }
}
