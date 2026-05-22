using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class UpdateEventDto
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tarih zorunludur.")]
    public DateTime Date { get; set; }

    [Range(1, 100000, ErrorMessage = "Kapasite 1 ile 100000 arasında olmalıdır.")]
    public int Capacity { get; set; }

    [Range(0, 999999.99, ErrorMessage = "Geçerli bir fiyat giriniz.")]
    public decimal TicketPrice { get; set; }

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

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir mekan seçiniz.")]
    public int VenueId { get; set; }

    public int? ArtistId { get; set; }
}