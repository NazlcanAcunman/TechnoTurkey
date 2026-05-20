using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class UsersController : Controller
{
    private readonly IAdminUlService _adminService;

    public UsersController(IAdminUlService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Kullanıcılar";
        var users = await _adminService.GetAllUsersAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _adminService.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AssignAdmin(string id)
    {
        await _adminService.AssignAdminAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AssignMember(string id)
    {
        await _adminService.AssignMemberAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
