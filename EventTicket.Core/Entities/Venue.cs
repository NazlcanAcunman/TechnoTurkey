namespace EventTicket.Core.Entities;

public class Venue : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;  
    public bool IsApproved { get; set; } = false;
    public string AdminUserId { get; set; } = string.Empty;

    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();  
}
