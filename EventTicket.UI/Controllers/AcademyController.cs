using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class AcademyController : BaseController
{
    private readonly IAcademyUlService _academyService;

    public AcademyController(IAcademyUlService academyService, IProfileUlService profileService)
        : base(profileService)
    {
        _academyService = academyService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _academyService.GetAllAsync();
        ViewData["Title"] = "Akademi";
        return View(items);
    }
}
