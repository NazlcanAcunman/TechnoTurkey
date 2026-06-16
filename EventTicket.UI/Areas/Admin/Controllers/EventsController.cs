using EventTicket.Core.DTOs;
using EventTicket.UI.Services;
using EventTicket.UI.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class EventsController : Controller
{
    private readonly IAdminUlService _adminService;
    private readonly IVenueUlService _venueService;
    private readonly IArtistUlService _artistService;

    public EventsController(
        IAdminUlService adminService,
        IVenueUlService venueService,
        IArtistUlService artistService)
    {
        _adminService = adminService;
        _venueService = venueService;
        _artistService = artistService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Etkinlikler";
        var events = await _adminService.GetAllEventsAsync();
        if (!events.Any())
            TempData["ApiError"] = _adminService.LastError ?? "API boş liste döndü";
        return View(events);
    }

    // GET: Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Yeni Etkinlik Ekle";
        var model = new CreateEventViewModel
        {
            Venues  = await _venueService.GetApprovedVenuesAsync(),
            Artists = await _artistService.GetApprovedArtistsAsync()
        };
        return View(model);
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEventViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Venues  = await _venueService.GetApprovedVenuesAsync();
            model.Artists = await _artistService.GetApprovedArtistsAsync();
            return View(model);
        }

        var dto = new CreateEventDto
        {
            Title             = model.Title,
            Description       = model.Description,
            Date              = model.Date,
            ImageUrl          = model.ImageUrl,
            TicketUrl         = model.TicketUrl,
            VenueId           = model.VenueId,
            ArtistId          = model.ArtistId,
            PromoCode         = string.IsNullOrWhiteSpace(model.PromoCode) ? null : model.PromoCode.ToUpperInvariant(),
            DiscountPercent   = model.DiscountPercent,
            PromoCodeColor    = model.PromoCodeColor,
            BadgeText         = string.IsNullOrWhiteSpace(model.BadgeText) ? null : model.BadgeText,
            BadgeColor        = model.BadgeColor,
            IsFeatured        = model.IsFeatured,
            IsGuestlistOpen   = model.IsGuestlistOpen,
            GuestlistClosesAt = model.GuestlistClosesAt,
            DressCode         = model.DressCode
        };

        try
        {
            await _adminService.CreateEventAsync(dto);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            model.Venues  = await _venueService.GetApprovedVenuesAsync();
            model.Artists = await _artistService.GetApprovedArtistsAsync();
            return View(model);
        }
        TempData["Success"] = "Etkinlik başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Etkinliği Düzenle";
        var ev = await _adminService.GetEventByIdAsync(id);
        if (ev == null) return NotFound();

        var model = new EditEventViewModel
        {
            Id                = ev.Id,
            Title             = ev.Title,
            Description       = ev.Description,
            Date              = ev.Date,
            ImageUrl          = ev.ImageUrl,
            TicketUrl         = ev.TicketUrl,
            VenueId           = ev.VenueId,
            ArtistId          = ev.ArtistId,
            PromoCode         = ev.PromoCode,
            DiscountPercent   = ev.DiscountPercent,
            PromoCodeColor    = ev.PromoCodeColor,
            BadgeText         = ev.BadgeText,
            BadgeColor        = ev.BadgeColor,
            IsFeatured        = ev.IsFeatured,
            IsGuestlistOpen   = ev.IsGuestlistOpen,
            GuestlistClosesAt = ev.GuestlistClosesAt,
            DressCode         = ev.DressCode,
            Venues            = await _venueService.GetApprovedVenuesAsync(),
            Artists           = await _artistService.GetApprovedArtistsAsync()
        };
        return View(model);
    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditEventViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Venues  = await _venueService.GetApprovedVenuesAsync();
            model.Artists = await _artistService.GetApprovedArtistsAsync();
            return View(model);
        }

        var dto = new UpdateEventDto
        {
            Title             = model.Title,
            Description       = model.Description,
            Date              = model.Date,
            ImageUrl          = model.ImageUrl,
            TicketUrl         = model.TicketUrl,
            VenueId           = model.VenueId,
            ArtistId          = model.ArtistId,
            PromoCode         = string.IsNullOrWhiteSpace(model.PromoCode) ? null : model.PromoCode.ToUpperInvariant(),
            DiscountPercent   = model.DiscountPercent,
            PromoCodeColor    = model.PromoCodeColor,
            BadgeText         = string.IsNullOrWhiteSpace(model.BadgeText) ? null : model.BadgeText,
            BadgeColor        = model.BadgeColor,
            IsFeatured        = model.IsFeatured,
            IsGuestlistOpen   = model.IsGuestlistOpen,
            GuestlistClosesAt = model.GuestlistClosesAt,
            DressCode         = model.DressCode
        };

        try
        {
            await _adminService.UpdateEventAsync(id, dto);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            model.Venues  = await _venueService.GetApprovedVenuesAsync();
            model.Artists = await _artistService.GetApprovedArtistsAsync();
            return View(model);
        }
        TempData["Success"] = "Etkinlik başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    // POST: Restore (silmeyi geri al)
    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Restore(int id)
    {
        await _adminService.RestoreEventAsync(id);
        TempData["Success"] = "Etkinlik yeniden aktifleştirildi.";
        return RedirectToAction("Index");
    }

    // POST: Delete (soft)
    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminService.DeleteEventAsync(id);
        TempData["Success"] = "Etkinlik silindi.";
        return RedirectToAction("Index");
    }

    // POST: HardDelete (kalıcı)
    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> HardDelete(int id)
    {
        await _adminService.HardDeleteEventAsync(id);
        if (_adminService.LastError != null)
            TempData["ApiError"] = _adminService.LastError;
        else
            TempData["Success"] = "Etkinlik listeden kalıcı olarak kaldırıldı.";
        return RedirectToAction("Index");
    }
}
