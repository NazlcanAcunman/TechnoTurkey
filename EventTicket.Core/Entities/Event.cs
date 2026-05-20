namespace EventTicket.Core.Entities;

public class Event : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TicketPrice { get; set; }
    public int Capacity { get; set; }
    public int SoldCount { get; set; } = 0;
    public bool IsApproved { get; set; } = false;
    public string ImageUrl { get; set; } = string.Empty;
    public string? TicketUrl { get; set; }
    public string? RejectionReason { get; set; }
    public string? PromoCode { get; set; }
    public int? DiscountPercent { get; set; }
    public string? PromoCodeColor { get; set; }

    public int VenueId { get; set; }
    public int ArtistId { get; set; }

    public Venue Venue { get; set; } = null!;
    public Artist Artist { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}


