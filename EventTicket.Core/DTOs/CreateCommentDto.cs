using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class CreateCommentDto
{
    [Required(ErrorMessage = "Yorum içeriği zorunludur.")]
    [StringLength(1000, MinimumLength = 2, ErrorMessage = "Yorum 2 ile 1000 karakter arasında olmalıdır.")]
    public string Content { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
    public int Rating { get; set; }

    public int? EventId { get; set; }
    public int? VenueId { get; set; }
}