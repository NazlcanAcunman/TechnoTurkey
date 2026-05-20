using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace EventTicket.UI.Controllers;

public class AuthController : Controller
{
    private readonly IAuthUlService _authService;
    private readonly ICartService _cartService;

    public AuthController(IAuthUlService authService, ICartService cartService)
    {
        _authService = authService;
        _cartService = cartService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.LoginAsync(model.Email, model.Password);

        if (result == null || string.IsNullOrEmpty(result.Token))
        {
            model.ErrorMessage = ParseApiError(_authService.LastError) ?? "E-posta veya şifre hatalı.";
            return View(model);
        }

        await SignInUserAsync(result);

        if (result.Role == "SuperAdmin" || result.Role == "Admin")
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.RegisterAsync(
            model.FullName, model.Username, model.Email, model.Password,
            model.TcKimlikNo, model.DateOfBirth);

        if (result == null || string.IsNullOrEmpty(result.Token))
        {
            model.ErrorMessage = ParseApiError(_authService.LastError) ?? "Kayıt sırasında hata oluştu.";
            return View(model);
        }

        await SignInUserAsync(result);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        _cartService.ClearCart();
        await HttpContext.SignOutAsync("Cookies");
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("Index", "Home");
    }

    private static string? ParseApiError(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        try
        {
            var trimmed = raw.Trim();

            // Düz string: "Hata mesajı" ya da \"Hata mesajı\"
            if (!trimmed.StartsWith("{") && !trimmed.StartsWith("["))
                return trimmed.Trim('"');

            // JSON array: ["err1","err2"]
            if (trimmed.StartsWith("["))
            {
                var arr = JsonSerializer.Deserialize<string[]>(trimmed);
                return arr != null && arr.Length > 0 ? string.Join(" ", arr) : null;
            }

            // ProblemDetails: {"errors":{"Field":["msg1"]}, "title":"..."}
            if (trimmed.StartsWith("{"))
            {
                using var doc = JsonDocument.Parse(trimmed);
                var root = doc.RootElement;

                // FluentValidation 422: errors nesnesi
                if (root.TryGetProperty("errors", out var errorsEl) && errorsEl.ValueKind == JsonValueKind.Object)
                {
                    var msgs = new List<string>();
                    foreach (var field in errorsEl.EnumerateObject())
                        foreach (var msg in field.Value.EnumerateArray())
                            if (msg.GetString() is { } s) msgs.Add(s);
                    if (msgs.Count > 0) return string.Join(" ", msgs);
                }

                // Basit {message:"..."}, {error:"..."} ya da {title:"..."}
                if (root.TryGetProperty("message", out var msgEl) && msgEl.GetString() is { } message)
                    return message;
                if (root.TryGetProperty("error", out var errEl) && errEl.GetString() is { } error)
                    return error;
                if (root.TryGetProperty("title", out var titleEl) && titleEl.GetString() is { } title
                    && !title.Contains("validation", StringComparison.OrdinalIgnoreCase))
                    return title;
            }
        }
        catch { }
        return null;
    }

    private async Task SignInUserAsync(AuthResponseViewModel result)
    {
        Response.Cookies.Append("jwt_token", result.Token, new CookieOptions
        {
            HttpOnly = true,
            Expires = result.ExpiresAt,
            Secure = true,
            SameSite = SameSiteMode.Lax
        });

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,  result.UserName),
            new Claim(ClaimTypes.Email, result.Email),
            new Claim(ClaimTypes.Role,  result.Role),
            new Claim("jwt_token",      result.Token)
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = result.ExpiresAt
        });
    }
}