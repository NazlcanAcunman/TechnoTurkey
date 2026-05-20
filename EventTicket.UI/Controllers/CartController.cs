using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class CartController : BaseController
{
    private readonly ICartService _cartService;
    private readonly IEventUlService _eventService;
    private readonly ITicketUlService _ticketService;

    public CartController(
        ICartService cartService,
        IEventUlService eventService,
        ITicketUlService ticketService,
        IProfileUlService profileService)
        : base(profileService)
    {
        _cartService = cartService;
        _eventService = eventService;
        _ticketService = ticketService;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Sepetim";
        var cart = _cartService.GetCart();
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int eventId, int quantity = 1, bool goToCheckout = false)
    {
        if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
        {
            TempData["CartError"] = "Yönetici hesaplarıyla bilet satın alınamaz.";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        var ev = await _eventService.GetEventByIdAsync(eventId);
        if (ev == null)
            return NotFound();

        var available = ev.Capacity - ev.SoldCount;
        if (quantity > available)
        {
            TempData["CartError"] = $"Yalnızca {available} bilet mevcut.";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        var item = new CartItemViewModel
        {
            EventId = ev.Id,
            EventTitle = ev.Title,
            VenueName = ev.VenueName,
            EventDate = ev.Date,
            TicketPrice = ev.TicketPrice,
            Quantity = quantity,
            ImageUrl = ev.ImageUrl,
            AgeRestriction = ev.AgeRestriction
        };

        _cartService.AddToCart(item);

        if (goToCheckout)
            return RedirectToAction("Checkout");

        TempData["CartSuccess"] = $"'{ev.Title}' sepete eklendi!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int eventId, int quantity)
    {
        _cartService.UpdateQuantity(eventId, quantity);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int eventId)
    {
        _cartService.RemoveFromCart(eventId);
        TempData["CartSuccess"] = "Ürün sepetten kaldırıldı.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Clear()
    {
        _cartService.ClearCart();
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Checkout()
    {
        if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
        {
            TempData["CartError"] = "Yönetici hesaplarıyla bilet satın alınamaz.";
            return RedirectToAction("Index", "Home");
        }

        var cart = _cartService.GetCart();
        if (!cart.Items.Any())
        {
            TempData["CartError"] = "Sepetiniz boş.";
            return RedirectToAction("Index");
        }

        var model = new CartCheckoutViewModel
        {
            Cart = cart
        };

        ViewData["Title"] = "Ödeme";
        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(CartCheckoutViewModel model)
    {
        var cart = _cartService.GetCart();
        if (!cart.Items.Any())
            return RedirectToAction("Index");

        bool hasError = false;
        foreach (var item in cart.Items)
        {
            var result = await _ticketService.PurchaseAsync(item.EventId, item.Quantity);
            if (result == null || !result.Any())
            {
                TempData["CartError"] = $"'{item.EventTitle}' için bilet satın alınırken hata oluştu.";
                hasError = true;
                break;
            }
        }

        if (!hasError)
        {
            _cartService.ClearCart();
            TempData["PurchaseSuccess"] = "Tüm biletleriniz başarıyla satın alındı!";
            return RedirectToAction("Index", "Profile");
        }

        model.Cart = cart;
        return View(model);
    }
}