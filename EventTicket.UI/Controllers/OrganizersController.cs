using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class OrganizersController : BaseController
{
    public OrganizersController(IProfileUlService profileService) : base(profileService) { }

    public IActionResult Index()
    {
        ViewData["Title"] = "Organizatörler";
        return View();
    }
}
