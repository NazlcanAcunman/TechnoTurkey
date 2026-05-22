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

    [Range(1, 100000, ErrorMessage = "Kapasite en az 1 olmalıdır.")]
    public int Capacity { get; set; } = 100;

    [Range(0, 999999.99, ErrorMessage = "Geçerli bir fiyat giriniz.")]
    public decimal TicketPrice { get; set; }

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

    // Dropdown için
    public List<VenueCardViewModel> Venues { get; set; } = new();
    public List<ArtistCardViewModel> Artists { get; set; } = new();
}
