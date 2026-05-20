namespace EventTicket.UI.ViewModels;

public class ArtistCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
}