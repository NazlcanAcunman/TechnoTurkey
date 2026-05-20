using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace EventTicket.UI.Controllers;

[Authorize]
public class ProfileController : BaseController
{
    private readonly ITicketUlService _ticketService;
    private readonly IProfileUlService _profileService;
    private readonly IWebHostEnvironment _env;

    public ProfileController(ITicketUlService ticketService, IProfileUlService profileService, IWebHostEnvironment env)
        : base(profileService)
    {
        _ticketService = ticketService;
        _profileService = profileService;
        _env = env;
    }

    private string GetProfilePicturePath()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? "user";
        var safe = string.Concat(email.Where(c => char.IsLetterOrDigit(c)));
        var file = $"{safe}.jpg";
        var abs = Path.Combine(_env.WebRootPath, "uploads", "profiles", file);
        return System.IO.File.Exists(abs) ? $"/uploads/profiles/{file}" : string.Empty;
    }

    private string GetUserSettingsPath()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? "user";
        var safe = string.Concat(email.Where(c => char.IsLetterOrDigit(c)));
        return Path.Combine(_env.WebRootPath, "uploads", "profiles", $"{safe}.settings.json");
    }

    private (string Nickname, string Phone, string FirstName, string LastName) LoadUserSettings()
    {
        var path = GetUserSettingsPath();
        if (!System.IO.File.Exists(path)) return (string.Empty, string.Empty, string.Empty, string.Empty);
        try
        {
            var json = System.IO.File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            var nick = doc.RootElement.TryGetProperty("nickname", out var n) ? n.GetString() ?? "" : "";
            var phone = doc.RootElement.TryGetProperty("phone", out var p) ? p.GetString() ?? "" : "";
            var firstName = doc.RootElement.TryGetProperty("firstName", out var fn) ? fn.GetString() ?? "" : "";
            var lastName = doc.RootElement.TryGetProperty("lastName", out var ln) ? ln.GetString() ?? "" : "";
            return (nick, phone, firstName, lastName);
        }
        catch { return (string.Empty, string.Empty, string.Empty, string.Empty); }
    }

    public async Task<IActionResult> Index()
    {
        var fullName = User.FindFirstValue(ClaimTypes.Name) ?? "";
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        var email = User.FindFirstValue(ClaimTypes.Email) ?? "";

        // API, admin hesaplarda FullName yerine rol benzeri değer döndürebilir
        var isRoleLikeName = string.IsNullOrWhiteSpace(fullName)
            || fullName == role
            || fullName.Contains("Admin", StringComparison.OrdinalIgnoreCase)
            || fullName.Contains("Super", StringComparison.OrdinalIgnoreCase)
            || fullName.Contains("Süper", StringComparison.OrdinalIgnoreCase);

        if (isRoleLikeName)
        {
            var prefix = email.Contains('@') ? email.Split('@')[0] : email;
            // İlk harfi büyük yap
            fullName = prefix.Length > 0
                ? char.ToUpperInvariant(prefix[0]) + prefix.Substring(1)
                : prefix;
        }

        var (nickname, phone, firstName, lastName) = LoadUserSettings();

        var model = new ProfileViewModel
        {
            FullName = fullName,
            Email = email,
            ProfilePictureUrl = GetProfilePicturePath(),
            Nickname = nickname,
            Phone = phone,
            FirstName = firstName,
            LastName = lastName,
            Tickets = await _ticketService.GetMyTicketsAsync(),
            Favorites = await _profileService.GetFavoritesAsync(),
            Notifications = await _profileService.GetNotificationsAsync()
        };

        ViewData["Title"] = "Profilim";
        return View(model);
    }

    public IActionResult MyTickets() => RedirectToAction("Index");

    [HttpPost]
    public async Task<IActionResult> RemoveFavorite(int id)
    {
        await _profileService.RemoveFavoriteAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFavoriteByEntity(int? artistId, int? venueId)
    {
        var favorites = await _profileService.GetFavoritesAsync();
        var fav = favorites.FirstOrDefault(f =>
            (artistId.HasValue && f.ArtistId == artistId) ||
            (venueId.HasValue && f.VenueId == venueId));
        if (fav != null)
            await _profileService.RemoveFavoriteAsync(fav.Id);
        return Ok(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> AddFavorite(int? artistId, int? venueId, int? returnEventId)
    {
        if (artistId.HasValue && artistId > 0)
            await _profileService.AddFavoriteAsync(null, artistId);
        if (venueId.HasValue && venueId > 0)
            await _profileService.AddFavoriteAsync(venueId, null);
        if (returnEventId.HasValue)
            return RedirectToAction("Details", "Events", new { id = returnEventId.Value });
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> MarkNotificationRead(int id)
    {
        await _profileService.MarkNotificationReadAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateSettings(string? nickname, string? phone, string? firstName, string? lastName)
    {
        // Nickname doğrulama: 6-12 karakter, en az bir harf
        if (!string.IsNullOrEmpty(nickname))
        {
            if (nickname.Length < 6 || nickname.Length > 12)
            {
                TempData["SettingsError"] = "Kullanıcı adı 6-12 karakter arasında olmalıdır.";
                return RedirectToAction("Index", new { tab = "settings" });
            }
            if (!nickname.Any(char.IsLetter))
            {
                TempData["SettingsError"] = "Kullanıcı adı en az bir harf içermelidir.";
                return RedirectToAction("Index", new { tab = "settings" });
            }
        }

        var dir = Path.Combine(_env.WebRootPath, "uploads", "profiles");
        Directory.CreateDirectory(dir);

        var settings = new { nickname = nickname ?? "", phone = phone ?? "", firstName = firstName ?? "", lastName = lastName ?? "" };
        var json = JsonSerializer.Serialize(settings);
        System.IO.File.WriteAllText(GetUserSettingsPath(), json);

        TempData["SettingsSuccess"] = "Ayarlar kaydedildi.";
        return RedirectToAction("Index", new { tab = "settings" });
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhoto(IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
            return RedirectToAction("Index");

        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowed.Contains(photo.ContentType.ToLower()))
        {
            TempData["ProfileError"] = "Sadece JPG, PNG veya WEBP yükleyebilirsiniz.";
            return RedirectToAction("Index");
        }

        if (photo.Length > 5 * 1024 * 1024)
        {
            TempData["ProfileError"] = "Dosya boyutu 5 MB'ı aşamaz.";
            return RedirectToAction("Index");
        }

        var dir = Path.Combine(_env.WebRootPath, "uploads", "profiles");
        Directory.CreateDirectory(dir);

        var email = User.FindFirstValue(ClaimTypes.Email) ?? "user";
        var safe = string.Concat(email.Where(c => char.IsLetterOrDigit(c)));
        var file = Path.Combine(dir, $"{safe}.jpg");

        using var stream = new FileStream(file, FileMode.Create);
        await photo.CopyToAsync(stream);

        return RedirectToAction("Index");
    }
}