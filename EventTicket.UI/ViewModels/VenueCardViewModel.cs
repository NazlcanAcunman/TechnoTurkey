namespace EventTicket.UI.ViewModels;

public class VenueCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public string AdminUserId { get; set; } = string.Empty;
}