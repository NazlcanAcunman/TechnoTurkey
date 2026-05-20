using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class PurchaseTicketDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir etkinlik seçiniz.")]
    public int EventId { get; set; }

    [Range(1, 10, ErrorMessage = "Bir seferde en fazla 10 bilet satın alınabilir.")]
    public int Quantity { get; set; }

    [StringLength(100, ErrorMessage = "Koltuk tipi en fazla 100 karakter olabilir.")]
    public string? SeatType { get; set; }
}