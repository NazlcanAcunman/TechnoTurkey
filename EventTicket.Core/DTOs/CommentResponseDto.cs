namespace EventTicket.Core.DTOs;

public class CommentResponseDto
{
    public int Id { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? EventId { get; set; }
    public int? VenueId { get; set; }
}