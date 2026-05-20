using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class ArtistsController : Controller
{
    private readonly IAdminUlService _adminService;

    public ArtistsController(IAdminUlService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Sanatçılar";
        var artists = await _adminService.GetAllArtistsAsync();
        return View(artists);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminService.DeleteArtistAsync(id);
        return RedirectToAction("Index");
    }
}