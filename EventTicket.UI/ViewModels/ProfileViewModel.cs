namespace EventTicket.UI.ViewModels;

public class ProfileViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName => !string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName)
        ? $"{FirstName} {LastName}".Trim()
        : FullName;
    public List<TicketViewModel> Tickets { get; set; } = new();
    public List<FavoriteViewModel> Favorites { get; set; } = new();
    public List<NotificationViewModel> Notifications { get; set; } = new();
}

public class FavoriteViewModel
{
    public int Id { get; set; }
    public int? VenueId { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public int? ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
}

public class NotificationViewModel
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}