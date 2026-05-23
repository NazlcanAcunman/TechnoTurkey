using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class MagazineController : BaseController
{
    private readonly IArticleUlService _articleSvc;

    public MagazineController(IProfileUlService profileService, IArticleUlService articleSvc)
        : base(profileService)
    {
        _articleSvc = articleSvc;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Magazin";
        var articles = await _articleSvc.GetPublishedAsync();
        return View(articles);
    }

    [Route("Magazine/Details/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var article = await _articleSvc.GetBySlugAsync(slug);
        if (article == null) return NotFound();
        ViewData["Title"] = article.Title + " - Magazin";
        return View(article);
    }
}
