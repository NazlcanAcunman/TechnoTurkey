using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class BannersController : Controller
{
    private readonly IBannerUlService _bannerService;
    private readonly IAdminUlService _adminService;

    public BannersController(IBannerUlService bannerService, IAdminUlService adminService)
    {
        _bannerService = bannerService;
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Bannerlar";
        var banners = await _bannerService.GetAllAsync();
        return View(banners);
    }

    // GET: Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Yeni Banner Ekle";
        var model = new BannerViewModel { EventOptions = await GetEventOptionsAsync() };
        return View(model);
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BannerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.EventOptions = await GetEventOptionsAsync();
            return View(model);
        }

        await _bannerService.CreateAsync(model);
        TempData["Success"] = "Banner başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Banner Düzenle";
        var banner = await _bannerService.GetByIdAsync(id);
        if (banner == null) return NotFound();
        banner.EventOptions = await GetEventOptionsAsync();
        return View(banner);
    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BannerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.EventOptions = await GetEventOptionsAsync();
            return View(model);
        }

        await _bannerService.UpdateAsync(id, model);
        TempData["Success"] = "Banner başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    // POST: Delete
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _bannerService.DeleteAsync(id);
        TempData["Success"] = "Banner silindi.";
        return RedirectToAction("Index");
    }

    private async Task<List<SelectListItem>> GetEventOptionsAsync()
    {
        var events = await _adminService.GetAllEventsAsync();
        var options = events
            .OrderBy(e => e.Title)
            .Select(e => new SelectListItem(e.Title, e.Id.ToString()))
            .ToList();
        options.Insert(0, new SelectListItem("— Seçiniz —", ""));
        return options;
    }
}
