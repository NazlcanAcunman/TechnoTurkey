using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class CommentUlService : ICommentUlService
{
    private readonly IApiService _api;

    public CommentUlService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<CommentViewModel>> GetByEventAsync(int eventId)
    {
        return await _api.GetAsync<List<CommentViewModel>>($"api/comments/event/{eventId}")
               ?? new List<CommentViewModel>();
    }

    public async Task<List<CommentViewModel>> GetByVenueAsync(int venueId)
    {
        return await _api.GetAsync<List<CommentViewModel>>($"api/comments/venue/{venueId}")
               ?? new List<CommentViewModel>();
    }

    public async Task AddAsync(CreateCommentViewModel model)
    {
        await _api.PostAsync<object>("api/comments", new
        {
            content = model.Content,
            rating = model.Rating,
            eventId = model.EventId,
            venueId = model.VenueId
        });
    }

    public async Task DeleteAsync(int id)
    {
        await _api.DeleteAsync($"api/comments/{id}");
    }
}
