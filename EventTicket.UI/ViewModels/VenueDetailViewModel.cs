namespace EventTicket.UI.ViewModels;

public class VenueDetailViewModel
{
    public VenueCardViewModel Venue { get; set; } = new();
    public List<CommentViewModel> Comments { get; set; } = new();
    public CreateCommentViewModel NewComment { get; set; } = new();
}