using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface ICommentUlService
{
    Task<List<CommentViewModel>> GetByEventAsync(int eventId);
    Task<List<CommentViewModel>> GetByVenueAsync(int venueId);
    Task AddAsync(CreateCommentViewModel model);
    Task DeleteAsync(int id);
}