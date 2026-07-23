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
    private readonly IApiService _apiService;

    public AcademyController(IAcademyUlService academyService, IApiService apiService)
    {
        _academyService = academyService;
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _academyService.GetAllAsync();
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Json(new { success = false, message = "Dosya seçilmedi." });

        var result = await _apiService.PostFileAsync<UploadResultDto>("api/upload/image", file);
        if (result == null || string.IsNullOrEmpty(result.Url))
            return Json(new { success = false, message = _apiService.LastError ?? "Yükleme başarısız." });

        return Json(new { success = true, url = result.Url });
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

public class UploadResultDto
{
    public string? Url { get; set; }
}
