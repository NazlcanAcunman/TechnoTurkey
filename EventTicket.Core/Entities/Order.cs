namespace EventTicket.Core.Entities;

public class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public string? PaymentIntentId { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
