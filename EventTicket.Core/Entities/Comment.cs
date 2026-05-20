namespace EventTicket.Core.Entities;

public class Comment : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }

    public int? EventId { get; set; }
    public int? VenueId { get; set; }

    public Event? Event { get; set; }
    public Venue? Venue { get; set; }
}
