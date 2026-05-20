namespace EventTicket.UI.ViewModels;

public class TicketViewModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string QRCode { get; set; } = string.Empty;
    public string SeatInfo { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
}