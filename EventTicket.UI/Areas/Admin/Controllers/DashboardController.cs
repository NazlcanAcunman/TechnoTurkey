using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class DashboardController : Controller
{
    private readonly IAdminUlService _adminService;

    public DashboardController(IAdminUlService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        var stats = await _adminService.GetStatsAsync();
        var pending = await _adminService.GetPendingAsync();
        ViewBag.PendingCount = pending.Events.Count + pending.Venues.Count + pending.Artists.Count;
        return View(stats);
    }
}