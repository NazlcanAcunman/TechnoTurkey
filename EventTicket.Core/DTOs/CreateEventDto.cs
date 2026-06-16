using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class CreateEventDto
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tarih zorunludur.")]
    public DateTime Date { get; set; }

    public int Capacity { get; set; } = 999999;

    public decimal TicketPrice { get; set; } = 0;

    [StringLength(500, ErrorMessage = "Görsel URL en fazla 500 karakter olabilir.")]
    public string ImageUrl { get; set; } = string.Empty;

    public string? TicketUrl { get; set; }
    public string? City { get; set; }

    [StringLength(20, ErrorMessage = "Promosyon kodu en fazla 20 karakter olabilir.")]
    [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Promosyon kodu sadece büyük harf ve rakam içerebilir.")]
    public string? PromoCode { get; set; }

    [Range(1, 99, ErrorMessage = "İndirim yüzdesi 1-99 arasında olmalıdır.")]
    public int? DiscountPercent { get; set; }

    public string? PromoCodeColor { get; set; }
    public string? BadgeText { get; set; }
    public string? BadgeColor { get; set; }
    public bool IsFeatured { get; set; } = false;
    public bool IsGuestlistOpen { get; set; } = false;
    public DateTime? GuestlistClosesAt { get; set; }
    public string? DressCode { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir mekan seçiniz.")]
    public int VenueId { get; set; }

    public int? ArtistId { get; set; }
}
