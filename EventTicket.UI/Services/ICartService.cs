using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface ICartService
{
    CartViewModel GetCart();
    void AddToCart(CartItemViewModel item);
    void UpdateQuantity(int eventId, int quantity);
    void RemoveFromCart(int eventId);
    void ClearCart();
    int GetCartCount();
}