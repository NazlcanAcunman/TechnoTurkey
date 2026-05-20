namespace EventTicket.UI.ViewModels;

public class CommentViewModel
{
    public int Id { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCommentViewModel
{
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public int? EventId { get; set; }
    public int? VenueId { get; set; }
}