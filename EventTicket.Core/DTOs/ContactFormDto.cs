using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class ContactFormDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(200, ErrorMessage = "Ad Soyad en fazla 200 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(256, ErrorMessage = "E-posta en fazla 256 karakter olabilir.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Konu en fazla 500 karakter olabilir.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mesaj zorunludur.")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "Mesaj 10 ile 5000 karakter arasında olmalıdır.")]
    public string Message { get; set; } = string.Empty;
}