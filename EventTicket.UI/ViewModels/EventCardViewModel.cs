namespace EventTicket.UI.ViewModels;

public class EventCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TicketPrice { get; set; }
    public int Capacity { get; set; }
    public int SoldCount { get; set; }
    public bool IsApproved { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? TicketUrl { get; set; }
    public int VenueId { get; set; }
    public int ArtistId { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string VenueCity { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string ArtistGenre { get; set; } = string.Empty;
    public bool AgeRestriction { get; set; }
    public string? PromoCode { get; set; }
    public int? DiscountPercent { get; set; }
    public string? PromoCodeColor { get; set; }
}