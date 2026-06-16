using EventTicket.Core.Enums;

namespace EventTicket.Core.DTOs;

public class UpdateGuestlistStatusDto
{
    public GuestlistStatus Status { get; set; }
    public string? AdminNote { get; set; }
}
