using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class VenuesController : Controller
{
    private readonly IAdminUlService _adminService;

    public VenuesController(IAdminUlService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Mekanlar";
        var venues = await _adminService.GetAllVenuesAsync();
        return View(venues);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminService.DeleteVenueAsync(id);
        return RedirectToAction("Index");
    }
}