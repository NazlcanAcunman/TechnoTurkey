using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class CreateArtistDto
{
    [Required(ErrorMessage = "Sanatçı adı zorunludur.")]
    [StringLength(200, ErrorMessage = "Sanatçı adı en fazla 200 karakter olabilir.")] 
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Biyografi en fazla 2000 karakter olabilir.")] 
    public string Bio { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tür zorunludur.")]
    [StringLength(100, ErrorMessage = "Tür en fazla 100 karakter olabilir.")] 
    public string Genre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Görsel URL en fazla 500 karakter olabilir.")] 
    public string ImageUrl { get; set; } = string.Empty;
}
