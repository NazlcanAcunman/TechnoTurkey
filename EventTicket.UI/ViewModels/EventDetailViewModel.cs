namespace EventTicket.UI.ViewModels;

public class EventDetailViewModel
{
    public EventCardViewModel Event { get; set; } = new();
    public List<CommentViewModel> Comments { get; set; } = new();
    public CreateCommentViewModel NewComment { get; set; } = new();
}