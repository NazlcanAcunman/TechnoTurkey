using EventTicket.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EventTicket.UI.ViewModels;

public class BannerViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık zorunludur.")]
    public string Title { get; set; } = string.Empty;

    public string SubTitle { get; set; } = string.Empty;
    public string? DateText { get; set; }
    public string? Venue { get; set; }

    [Required(ErrorMessage = "Görsel URL zorunludur.")]
    public string ImageUrl { get; set; } = string.Empty;

    public int? EventId { get; set; }
    public string? SearchQuery { get; set; }
    public string? TicketUrl { get; set; }

    public List<SelectListItem> EventOptions { get; set; } = new();
    public BannerType Type { get; set; } = BannerType.HeroSlide;
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public string? TagText { get; set; }
    public string? TextColor { get; set; }
    public string? BgColor { get; set; }
}
