using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicket.UI.ViewComponents;

/// <summary>
/// Navbar'daki kullanıcı alanını render eder (isim, rol badge, çıkış).
/// Kullanım: @await Component.InvokeAsync("UserNav")
/// </summary>
public class UserNavViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new UserNavViewModel
        {
            IsAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false,
            DisplayName     = HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            Email           = HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Role            = HttpContext.User.FindFirstValue(ClaimTypes.Role) ?? string.Empty
        };
        return View(model);
    }
}

public class UserNavViewModel
{
    public bool   IsAuthenticated { get; set; }
    public string DisplayName     { get; set; } = string.Empty;
    public string Email           { get; set; } = string.Empty;
    public string Role            { get; set; } = string.Empty;

    public bool IsAdmin => Role is "SuperAdmin" or "Admin";

    public string RoleBadge => Role switch
    {
        "SuperAdmin" => "YÖNETİCİ",
        "Admin"      => "ADMİN",
        "Member"     => "ÜYE",
        _            => "MİSAFİR"
    };
}
