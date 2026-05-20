using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;

namespace EventTicket.Core.Mappings;

public static class MappingExtensions
{
    public static EventResponseDto ToDto(this Event e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        Description = e.Description,
        Date = e.Date,
        TicketPrice = e.TicketPrice,
        Capacity = e.Capacity,
        SoldCount = e.SoldCount,
        ImageUrl = e.ImageUrl,
        IsApproved = e.IsApproved,
        VenueId = e.VenueId,
        VenueName = e.Venue?.Name ?? string.Empty,
        VenueCity = e.Venue?.City ?? string.Empty,
        ArtistId = e.ArtistId,
        ArtistName = e.Artist?.Name ?? string.Empty,
        ArtistGenre = e.Artist?.Genre ?? string.Empty,
        CreatedAt = e.CreatedAt  
    };

    public static VenueResponseDto ToDto(this Venue v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        Description = v.Description,
        City = v.City,
        Address = v.Address,
        IsApproved = v.IsApproved,
        AdminUserId = v.AdminUserId
    };

    public static ArtistResponseDto ToDto(this Artist a) => new()
    {
        Id = a.Id,
        Name = a.Name,
        Bio = a.Bio,
        Genre = a.Genre,
        ImageUrl = a.ImageUrl,
        IsApproved = a.IsApproved
    };

    public static CommentResponseDto ToDto(this Comment c) => new()
    {
        Id = c.Id,
        UserFullName = c.UserFullName,
        Content = c.Content,
        Rating = c.Rating,
        CreatedAt = c.CreatedAt,
        EventId = c.EventId,
        VenueId = c.VenueId
    };

    public static UserDto ToDto(this AppUser u, string role = "") => new()
    {
        Id = u.Id,
        UserName = u.UserName,
        Email = u.Email ?? string.Empty,
        Role = role,
        IsActive = u.IsActive,  
        CreatedAt = u.CreatedAt
    };

    public static TicketResponseDto ToDto(this Ticket t) => new()
    {
        Id = t.Id,
        EventId = t.EventId,
        OrderId = t.OrderId,
        EventTitle = t.Event?.Title ?? string.Empty,
        ArtistName = t.Event?.Artist?.Name ?? string.Empty,
        EventDate = t.Event?.Date ?? DateTime.MinValue,
        VenueName = t.Event?.Venue?.Name ?? string.Empty,
        VenueCity = t.Event?.Venue?.City ?? string.Empty,
        TicketPrice = t.Event?.TicketPrice ?? 0,
        QRCode = t.QRCode,
        SeatInfo = t.SeatInfo,
        PurchaseDate = t.CreatedAt
    };
}
