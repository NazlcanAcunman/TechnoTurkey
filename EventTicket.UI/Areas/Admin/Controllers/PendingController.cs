using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class PendingController : Controller
{
    private readonly IAdminUlService _adminService;

    public PendingController(IAdminUlService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Onay Bekleyenler";
        var pending = await _adminService.GetPendingAsync();
        return View(pending);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveEvent(int id)
    {
        await _adminService.ApproveEventAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ApproveVenue(int id)
    {
        await _adminService.ApproveVenueAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ApproveArtist(int id)
    {
        await _adminService.ApproveArtistAsync(id);
        return RedirectToAction("Index");
    }
}