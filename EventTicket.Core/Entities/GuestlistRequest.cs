using System.ComponentModel.DataAnnotations.Schema;
using EventTicket.Core.Enums;

namespace EventTicket.Core.Entities;

public class GuestlistRequest : BaseEntity
{
    public int EventId { get; set; }
    public string AddedByUserId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? Note { get; set; }
    public string? Gender { get; set; }
    public bool TermsAccepted { get; set; } = false;

    [Column(TypeName = "int")]
    public GuestlistStatus Status { get; set; } = GuestlistStatus.Pending;

    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? AdminNote { get; set; }

    // Navigation
    public Event Event { get; set; } = null!;
    public AppUser AddedByUser { get; set; } = null!;
}
