using System.ComponentModel.DataAnnotations;
using EventTicket.Core.Entities;

namespace EventTicket.Core.DTOs;

public class CreateBannerDto
{
    [Required] [StringLength(200)] public string Title { get; set; } = string.Empty;
    [StringLength(200)] public string SubTitle { get; set; } = string.Empty;
    [StringLength(50)] public string? DateText { get; set; }
    [StringLength(200)] public string? Venue { get; set; }
    [Required] [StringLength(500)] public string ImageUrl { get; set; } = string.Empty;
    public int? EventId { get; set; }
    [StringLength(200)] public string? SearchQuery { get; set; }
    [StringLength(500)] public string? TicketUrl { get; set; }
    public BannerType Type { get; set; } = BannerType.HeroSlide;
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    [StringLength(50)]  public string? TagText { get; set; }
    [StringLength(20)]  public string? TextColor { get; set; }
    [StringLength(20)]  public string? BgColor { get; set; }
}

public class UpdateBannerDto : CreateBannerDto { }

public class BannerResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public string? DateText { get; set; }
    public string? Venue { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int? EventId { get; set; }
    public string? SearchQuery { get; set; }
    public string? TicketUrl { get; set; }
    public BannerType Type { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? TagText { get; set; }
    public string? TextColor { get; set; }
    public string? BgColor { get; set; }
}
