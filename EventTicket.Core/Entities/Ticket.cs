namespace EventTicket.Core.Entities;

public class Ticket : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public int OrderId { get; set; }
    public string QRCode { get; set; } = Guid.NewGuid().ToString();
    public string SeatInfo { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;     
    public DateTime? UsedAt { get; set; }         

    public Event Event { get; set; } = null!;
    public Order Order { get; set; } = null!;
}