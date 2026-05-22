namespace EventTicket.Core.DTOs;

public class AdminStatsDto
{
    public int TotalEvents { get; set; }
    public int TotalVenues { get; set; }
    public int TotalArtists { get; set; }
    public int TotalTickets { get; set; }
    public int TotalUsers { get; set; }
    public int PendingEvents { get; set; }
    public int PendingVenues { get; set; }
    public int PendingArtists { get; set; }
    public long TotalPageViews { get; set; }
}
