using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class ArticlesController : Controller
{
    private readonly IArticleUlService _svc;

    public ArticlesController(IArticleUlService svc) => _svc = svc;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Makaleler";
        var articles = await _svc.GetAllAsync();
        return View(articles);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Yeni Makale";
        return View(new ArticleViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _svc.CreateAsync(new
        {
            model.Title,
            model.Summary,
            model.Content,
            model.ImageUrl,
            model.Tag,
            model.TagColor,
            model.Author,
            model.ReadTime,
            model.IsPublished
        });

        TempData["Success"] = "Makale oluşturuldu.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Makale Düzenle";
        var article = await _svc.GetByIdAsync(id);
        if (article == null) return NotFound();
        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _svc.UpdateAsync(id, new
        {
            model.Title,
            model.Summary,
            model.Content,
            model.ImageUrl,
            model.Tag,
            model.TagColor,
            model.Author,
            model.ReadTime,
            model.IsPublished
        });

        TempData["Success"] = "Makale güncellendi.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        TempData["Success"] = "Makale silindi.";
        return RedirectToAction("Index");
    }
}
