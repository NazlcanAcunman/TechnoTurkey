namespace EventTicket.Core.DTOs;

public class GuestlistStatsDto
{
    public int TotalCount { get; set; }
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
}
