using EventTicket.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

/// <summary>
/// Kullanıcı hesap yönetimi — profil redirect, hesap silme.
/// Detaylı profil işlemleri için ProfileController'a yönlendirir.
/// </summary>
[Authorize]
public class AccountController : BaseController
{
    private readonly IAuthUlService _authService;
    private readonly ICartService _cartService;

    public AccountController(
        IAuthUlService authService,
        ICartService cartService,
        IProfileUlService profileService)
        : base(profileService)
    {
        _authService = authService;
        _cartService = cartService;
    }

    // GET /Account/Index → Profile'a yönlendir
    public IActionResult Index() => RedirectToAction("Index", "Profile");

    // POST /Account/DeleteAccount — Kurumsal üyelik için hesap silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount()
    {
        var success = await _authService.DeleteAccountAsync();

        if (!success)
        {
            TempData["Error"] = "Hesap silinirken bir hata oluştu. Lütfen tekrar deneyin.";
            return RedirectToAction("Index", "Profile");
        }

        // Oturumu kapat
        _cartService.ClearCart();
        await HttpContext.SignOutAsync("Cookies");
        Response.Cookies.Delete("jwt_token");

        TempData["AccountDeleted"] = "Hesabınız başarıyla silindi.";
        return RedirectToAction("Index", "Home");
    }

    // GET /Account/DeleteConfirm — Onay sayfası
    public IActionResult DeleteConfirm() => View();
}
