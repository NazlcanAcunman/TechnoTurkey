using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class RejectEventDto
{
    [Required(ErrorMessage = "Red sebebi zorunludur.")]
    [StringLength(500, ErrorMessage = "Red sebebi en fazla 500 karakter olabilir.")]
    public string Reason { get; set; } = string.Empty;
}
