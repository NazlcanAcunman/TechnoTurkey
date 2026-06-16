using EventTicket.Core.Enums;

namespace EventTicket.Core.DTOs;

public class CreateGuestlistRequestDto
{
    public int EventId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Note { get; set; }
    public GuestlistType GuestlistType { get; set; } = GuestlistType.Free;
}
