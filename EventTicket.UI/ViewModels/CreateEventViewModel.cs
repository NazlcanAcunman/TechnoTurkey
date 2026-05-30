using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EventTicket.UI.ViewModels.Admin;

public class CreateEventViewModel
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tarih zorunludur.")]
    public DateTime Date { get; set; } = DateTime.Now.AddDays(7);

    public string ImageUrl { get; set; } = string.Empty;
    public string? TicketUrl { get; set; }
    public string? City { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mekan seçiniz.")]
    public int VenueId { get; set; }

    public int? ArtistId { get; set; }

    public string? PromoCode { get; set; }
    public int? DiscountPercent { get; set; }
    public string? PromoCodeColor { get; set; }
    public string? BadgeText { get; set; }
    public string? BadgeColor { get; set; }
    public bool IsFeatured { get; set; } = false;

    // Dropdown için
    public List<VenueCardViewModel> Venues { get; set; } = new();
    public List<ArtistCardViewModel> Artists { get; set; } = new();
}
