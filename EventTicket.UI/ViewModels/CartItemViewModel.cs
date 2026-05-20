namespace EventTicket.UI.ViewModels;

public class CartItemViewModel
{
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public decimal TicketPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public string ImageUrl { get; set; } = string.Empty;
    public bool AgeRestriction { get; set; }
    public decimal SubTotal => TicketPrice * Quantity;
}
