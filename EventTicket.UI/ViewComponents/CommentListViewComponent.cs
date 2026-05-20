using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.ViewComponents;

/// <summary>
/// Bir etkinlik veya mekan için yorum listesini render eder.
/// Kullanım: @await Component.InvokeAsync("CommentList", new { venueId = Model.Venue.Id })
///           @await Component.InvokeAsync("CommentList", new { eventId = Model.Event.Id })
/// </summary>
public class CommentListViewComponent : ViewComponent
{
    private readonly ICommentUlService _commentService;

    public CommentListViewComponent(ICommentUlService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? eventId = null, int? venueId = null)
    {
        List<CommentViewModel> comments;

        if (venueId.HasValue)
            comments = await _commentService.GetByVenueAsync(venueId.Value);
        else if (eventId.HasValue)
            comments = await _commentService.GetByEventAsync(eventId.Value);
        else
            comments = new List<CommentViewModel>();

        var model = new CommentListComponentViewModel
        {
            Comments = comments,
            NewComment = new CreateCommentViewModel
            {
                EventId = eventId,
                VenueId = venueId
            }
        };

        return View(model);
    }
}

public class CommentListComponentViewModel
{
    public List<CommentViewModel> Comments { get; set; } = new();
    public CreateCommentViewModel NewComment { get; set; } = new();
    public double AverageRating => Comments.Count > 0
        ? Math.Round(Comments.Average(c => c.Rating), 1)
        : 0;
}
