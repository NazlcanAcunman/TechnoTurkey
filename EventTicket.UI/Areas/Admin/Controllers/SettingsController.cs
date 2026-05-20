using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class SettingsController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Ayarlar";
        return View();
    }
}
