using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class AcademyController : Controller
{
    private readonly IAcademyUlService _academyService;
    public AcademyController(IAcademyUlService academyService) => _academyService = academyService;

    public async Task<IActionResult> Index()
    {
        var items = await _academyService.GetAllAsync();
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AcademyContentViewModel vm)
    {
        await _academyService.CreateAsync(vm);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _academyService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(AcademyContentViewModel vm)
    {
        await _academyService.UpdateAsync(vm.Id, vm);
        return RedirectToAction("Index");
    }
}
