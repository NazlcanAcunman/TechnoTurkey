namespace EventTicket.Core.DTOs;

public class TicketResponseDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int OrderId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string VenueCity { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public string QRCode { get; set; } = string.Empty;
    public string SeatInfo { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
}
