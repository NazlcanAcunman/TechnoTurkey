using EventTicket.Core.DTOs;
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

    [Authorize(Roles = "SuperAdmin")]
    public IActionResult Create() => View(new CreateVenueDto());

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Create(CreateVenueDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _adminService.CreateVenueAsync(dto);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(int id)
    {
        var venue = await _adminService.GetVenueByIdAsync(id);
        if (venue == null) return NotFound();
        return View(new UpdateVenueDto
        {
            Name = venue.Name,
            Description = venue.Description,
            City = venue.City,
            Address = venue.Address
        });
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(int id, UpdateVenueDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _adminService.UpdateVenueAsync(id, dto);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminService.DeleteVenueAsync(id);
        return RedirectToAction("Index");
    }
}