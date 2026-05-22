namespace EventTicket.Core.Entities;

public class PageView
{
    public int Id { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}
