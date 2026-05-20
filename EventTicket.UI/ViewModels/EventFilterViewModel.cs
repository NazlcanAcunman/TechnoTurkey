namespace EventTicket.UI.ViewModels;

public class EventFilterViewModel
{
    public List<EventCardViewModel> Events { get; set; } = new();
    public string? Search { get; set; }
    public string? City { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? ArtistName { get; set; }
    public string SortBy { get; set; } = "date";

    // Filtre seçenekleri için
    public List<string> AvailableCities { get; set; } = new();
    public List<string> AvailableArtists { get; set; } = new();
}