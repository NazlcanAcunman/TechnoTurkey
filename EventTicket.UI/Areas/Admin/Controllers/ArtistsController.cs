using EventTicket.Core.DTOs;
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

    [Authorize(Roles = "SuperAdmin")]
    public IActionResult Create() => View(new CreateArtistDto());

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Create(CreateArtistDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _adminService.CreateArtistAsync(dto);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(int id)
    {
        var artist = await _adminService.GetArtistByIdAsync(id);
        if (artist == null) return NotFound();
        return View(new UpdateArtistDto
        {
            Name = artist.Name,
            Bio = artist.Bio,
            Genre = artist.Genre,
            ImageUrl = artist.ImageUrl
        });
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(int id, UpdateArtistDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _adminService.UpdateArtistAsync(id, dto);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminService.DeleteArtistAsync(id);
        return RedirectToAction("Index");
    }
}