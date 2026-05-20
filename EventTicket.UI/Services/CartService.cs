using EventTicket.UI.ViewModels;
using System.Text.Json;

namespace EventTicket.UI.Services;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "EventTicket_Cart";

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public CartViewModel GetCart()
    {
        var json = Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json))
            return new CartViewModel();

        return JsonSerializer.Deserialize<CartViewModel>(json) ?? new CartViewModel();
    }

    private void SaveCart(CartViewModel cart)
    {
        Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }

    public void AddToCart(CartItemViewModel item)
    {
        var cart = GetCart();
        var existing = cart.Items.FirstOrDefault(i => i.EventId == item.EventId);

        if (existing != null)
            existing.Quantity += item.Quantity;
        else
            cart.Items.Add(item);

        SaveCart(cart);
    }

    public void UpdateQuantity(int eventId, int quantity)
    {
        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(i => i.EventId == eventId);

        if (item != null)
        {
            if (quantity <= 0)
                cart.Items.Remove(item);
            else
                item.Quantity = quantity;
        }

        SaveCart(cart);
    }

    public void RemoveFromCart(int eventId)
    {
        var cart = GetCart();
        cart.Items.RemoveAll(i => i.EventId == eventId);
        SaveCart(cart);
    }

    public void ClearCart()
    {
        Session.Remove(CartSessionKey);
    }

    public int GetCartCount()
    {
        return GetCart().TotalItems;
    }
}