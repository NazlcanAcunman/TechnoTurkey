namespace EventTicket.UI.ViewModels;

public class CartCheckoutViewModel
{
    public CartViewModel Cart { get; set; } = new();

    // Kart bilgileri
    public string CardHolderName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
}