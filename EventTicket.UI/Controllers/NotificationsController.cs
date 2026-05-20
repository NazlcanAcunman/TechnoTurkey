using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

[Authorize]
public class NotificationsController : BaseController
{
    private readonly INotificationUlService _notificationService;

    public NotificationsController(INotificationUlService notificationService, IProfileUlService profileService)
        : base(profileService)
    {
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var notifications = await _notificationService.GetMyNotificationsAsync();
        ViewData["Title"] = "Bildirimler";
        return View(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await _notificationService.MarkAllAsReadAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _notificationService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
