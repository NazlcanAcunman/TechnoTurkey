using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepo;

    public CommentService(IRepository<Comment> commentRepo)
    {
        _commentRepo = commentRepo;
    }

    public async Task<IEnumerable<CommentResponseDto>> GetByEventAsync(int eventId)
    {
        var comments = await _commentRepo.FindAsync(
            c => c.EventId == eventId && !c.IsDeleted);

        return comments.OrderByDescending(c => c.CreatedAt).Select(MapToDto);
    }

    public async Task<IEnumerable<CommentResponseDto>> GetByVenueAsync(int venueId)
    {
        var comments = await _commentRepo.FindAsync(
            c => c.VenueId == venueId && !c.IsDeleted);

        return comments.OrderByDescending(c => c.CreatedAt).Select(MapToDto);
    }

    public async Task<CommentResponseDto> AddAsync(CreateCommentDto dto, string userId, string userFullName)
    {
        if (dto.EventId == null && dto.VenueId == null)
            throw new ArgumentException("Etkinlik veya mekan belirtilmeli.");

        if (dto.Rating < 1 || dto.Rating > 5)
            throw new ArgumentException("Puan 1 ile 5 arasında olmalı.");

        var comment = new Comment
        {
            UserId = userId,
            UserFullName = userFullName,
            Content = dto.Content,
            Rating = dto.Rating,
            EventId = dto.EventId,
            VenueId = dto.VenueId
        };

        await _commentRepo.AddAsync(comment);
        return MapToDto(comment);
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Yorum bulunamadı.");

        comment.IsDeleted = true;
        await _commentRepo.UpdateAsync(comment);
    }

    private static CommentResponseDto MapToDto(Comment c) => new()
    {
        Id = c.Id,
        UserFullName = c.UserFullName,
        Content = c.Content,
        Rating = c.Rating,
        CreatedAt = c.CreatedAt,
        EventId = c.EventId,
        VenueId = c.VenueId
    };
}