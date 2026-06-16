using EventTicket.Core.Enums;

namespace EventTicket.Core.DTOs;

public class GuestlistRequestResponseDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string AddedByUserId { get; set; } = string.Empty;
    public string AddedByUserEmail { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? Note { get; set; }
    public string? Gender { get; set; }
    public bool TermsAccepted { get; set; }
    public GuestlistStatus Status { get; set; }
    public string? AdminNote { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
