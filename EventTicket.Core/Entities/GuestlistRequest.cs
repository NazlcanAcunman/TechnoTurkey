using System.ComponentModel.DataAnnotations.Schema;
using EventTicket.Core.Enums;

namespace EventTicket.Core.Entities;

public class GuestlistRequest : BaseEntity
{
    public int EventId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Note { get; set; }
    [Column(TypeName = "int")]
    public GuestlistStatus Status { get; set; } = GuestlistStatus.Pending;

    [Column(TypeName = "int")]
    public GuestlistType GuestlistType { get; set; } = GuestlistType.Free;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? AdminNote { get; set; }

    // Navigation
    public Event Event { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}
