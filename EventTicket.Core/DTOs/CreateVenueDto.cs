using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class CreateVenueDto
{
    [Required(ErrorMessage = "Mekan adı zorunludur.")]
    [StringLength(200, ErrorMessage = "Mekan adı en fazla 200 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")] 
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şehir zorunludur.")]
    [StringLength(100, ErrorMessage = "Şehir adı en fazla 100 karakter olabilir.")] 
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres zorunludur.")]
    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")] 
    public string Address { get; set; } = string.Empty;
}