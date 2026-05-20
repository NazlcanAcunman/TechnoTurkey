using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponseDto>> GetByEventAsync(int eventId);
    Task<IEnumerable<CommentResponseDto>> GetByVenueAsync(int venueId);
    Task<CommentResponseDto> AddAsync(CreateCommentDto dto, string userId, string userFullName);
    Task DeleteAsync(int id);
}