using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class BannersController : Controller
{
    private readonly IBannerUlService _bannerService;

    public BannersController(IBannerUlService bannerService)
    {
        _bannerService = bannerService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Bannerlar";
        var banners = await _bannerService.GetAllAsync();
        return View(banners);
    }

    // GET: Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Yeni Banner Ekle";
        return View(new BannerViewModel());
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BannerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

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
        return View(banner);
    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BannerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

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
}
