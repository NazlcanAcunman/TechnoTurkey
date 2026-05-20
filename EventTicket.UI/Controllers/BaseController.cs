using EventTicket.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;

namespace EventTicket.UI.Controllers;

public class BaseController : Controller
{
    private readonly IProfileUlService _profileService;
    private readonly IWebHostEnvironment _env;

    public BaseController(IProfileUlService profileService, IWebHostEnvironment? env = null)
    {
        _profileService = profileService;
        _env = env!;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var notifications = await _profileService.GetNotificationsAsync();
            ViewBag.UnreadNotifications = notifications.Count(n => !n.IsRead);

            // Ad-soyad: ayarlar dosyasından oku, yoksa claim'den al
            ViewBag.DisplayName = ResolveDisplayName();
        }
        else
        {
            ViewBag.UnreadNotifications = 0;
            ViewBag.DisplayName = "";
        }

        await next();
    }

    private string ResolveDisplayName()
    {
        var claimName = User.FindFirstValue(ClaimTypes.Name) ?? "";
        var role      = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var email     = User.FindFirstValue(ClaimTypes.Email) ?? "";

        // Ayarlar dosyasından firstName/lastName oku
        if (_env != null)
        {
            try
            {
                var safe = string.Concat(email.Where(char.IsLetterOrDigit));
                var path = Path.Combine(_env.WebRootPath, "uploads", "profiles", $"{safe}.settings.json");
                if (System.IO.File.Exists(path))
                {
                    var doc = JsonDocument.Parse(System.IO.File.ReadAllText(path));
                    var fn = doc.RootElement.TryGetProperty("firstName", out var f) ? f.GetString() ?? "" : "";
                    var ln = doc.RootElement.TryGetProperty("lastName",  out var l) ? l.GetString() ?? "" : "";
                    var full = $"{fn} {ln}".Trim();
                    if (!string.IsNullOrWhiteSpace(full)) return full;
                }
            }
            catch { /* dosya okunamazsa devam et */ }
        }

        // Claim'den gelen isim rol benzeri bir değerse e-posta önekini kullan
        var isRoleLike = string.IsNullOrWhiteSpace(claimName)
            || claimName == role
            || claimName.Contains("Admin", StringComparison.OrdinalIgnoreCase)
            || claimName.Contains("Super", StringComparison.OrdinalIgnoreCase)
            || claimName.Contains("Süper", StringComparison.OrdinalIgnoreCase);

        if (isRoleLike)
        {
            var prefix = email.Contains('@') ? email.Split('@')[0] : email;
            return prefix.Length > 0
                ? char.ToUpperInvariant(prefix[0]) + prefix.Substring(1)
                : prefix;
        }

        return claimName;
    }
}