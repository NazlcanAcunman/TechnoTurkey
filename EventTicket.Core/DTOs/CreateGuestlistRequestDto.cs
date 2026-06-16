namespace EventTicket.Core.DTOs;

public class CreateGuestlistRequestDto
{
    public int EventId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? Note { get; set; }
    public string? Gender { get; set; }
    public bool TermsAccepted { get; set; }
}
