namespace EventTicket.Core.Entities;

public enum BannerType { HeroSlide = 0, FeaturedBanner = 1, TopBar = 2 }

public class Banner : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public string? DateText { get; set; }   // "6-7 Haziran 2026"
    public string? Venue { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? SearchQuery { get; set; }
    public string? TicketUrl { get; set; }
    public BannerType Type { get; set; } = BannerType.HeroSlide;
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    // TopBar alanları
    public string? TagText { get; set; }       // "YENİ", "HOT" vb.
    public string? TextColor { get; set; }     // "#ffffff"
    public string? BgColor { get; set; }       // "#6c5ce7"
}
