namespace EventTicket.UI.ViewModels;

public class CheckoutViewModel
{
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public decimal TicketPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public int AvailableTickets { get; set; }

    // Sahte kart bilgileri — gerçek ödeme yok
    public string CardHolderName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;

    public decimal TotalAmount => TicketPrice * Quantity;
}