using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class EventService : IEventService
{
    private readonly IRepository<Event> _eventRepo;
    private readonly IRepository<Venue> _venueRepo;
    private readonly IRepository<Favorite> _favoriteRepo;
    private readonly IRepository<Notification> _notificationRepo;
    private readonly ILogger<EventService> _logger;

    public EventService(
        IRepository<Event> eventRepo,
        IRepository<Venue> venueRepo,
        IRepository<Favorite> favoriteRepo,
        IRepository<Notification> notificationRepo,
        ILogger<EventService> logger)
    {
        _eventRepo = eventRepo;
        _venueRepo = venueRepo;
        _favoriteRepo = favoriteRepo;
        _notificationRepo = notificationRepo;
        _logger = logger;
    }

    public async Task<IEnumerable<EventResponseDto>> GetAllApprovedAsync()
    {
        var now = DateTime.UtcNow;
        var events = await _eventRepo.FindWithIncludesAsync(
            e => e.IsApproved && !e.IsDeleted && e.Date >= now,
            e => e.Venue, e => e.Artist);
        return events.Select(MapToDto);
    }

    public async Task<IEnumerable<EventResponseDto>> GetAllForAdminAsync()
    {
        var events = await _eventRepo.FindWithIncludesAsync(
            e => !e.IsDeleted,
            e => e.Venue, e => e.Artist);
        return events.Select(MapToDto);
    }

    public async Task<IEnumerable<EventResponseDto>> GetAllPendingAsync()
    {
        var events = await _eventRepo.FindWithIncludesAsync(
            e => !e.IsApproved && !e.IsDeleted,
            e => e.Venue, e => e.Artist);
        return events.Select(MapToDto);
    }

    public async Task<IEnumerable<EventResponseDto>> GetByVenueAsync(int venueId)
    {
        var events = await _eventRepo.FindWithIncludesAsync(
            e => e.VenueId == venueId && e.IsApproved && !e.IsDeleted,
            e => e.Venue, e => e.Artist);
        return events.Select(MapToDto);
    }

    public async Task<EventResponseDto?> GetByIdAsync(int id)
    {
        var events = await _eventRepo.FindWithIncludesAsync(
            e => e.Id == id && !e.IsDeleted,
            e => e.Venue, e => e.Artist);
        var e = events.FirstOrDefault();
        return e == null ? null : MapToDto(e);
    }

    public async Task<EventResponseDto> CreateAsync(CreateEventDto dto, string adminUserId, bool isSuperAdmin)
    {
        var venue = await _venueRepo.GetByIdAsync(dto.VenueId)
            ?? throw new KeyNotFoundException("Mekan bulunamadı.");

        // SuperAdmin veya Admin doğrudan oluşturabilir ve otomatik onaylanır
        if (!isSuperAdmin && venue.AdminUserId != adminUserId)
            throw new UnauthorizedAccessException("Bu mekana etkinlik ekleme yetkiniz yok.");

        var newEvent = new Event
        {
            Title           = dto.Title,
            Description     = dto.Description,
            Date            = dto.Date,
            Capacity        = dto.Capacity,
            ImageUrl        = dto.ImageUrl,
            TicketUrl       = dto.TicketUrl,
            VenueId         = dto.VenueId,
            ArtistId        = dto.ArtistId,
            TicketPrice     = dto.TicketPrice,
            IsApproved      = true,
            PromoCode       = string.IsNullOrWhiteSpace(dto.PromoCode) ? null : dto.PromoCode.ToUpperInvariant(),
            DiscountPercent = dto.DiscountPercent,
            PromoCodeColor  = dto.PromoCodeColor
        };

        await _eventRepo.AddAsync(newEvent);
        _logger.LogInformation("Yeni etkinlik oluşturuldu: {Title} (ID: {Id})", newEvent.Title, newEvent.Id);
        return MapToDto(newEvent);
    }

    public async Task UpdateAsync(int id, UpdateEventDto dto, string adminUserId, bool isSuperAdmin)
    {
        var existing = await _eventRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Etkinlik bulunamadı.");

        if (!isSuperAdmin)
        {
            var venue = await _venueRepo.GetByIdAsync(existing.VenueId)
                ?? throw new KeyNotFoundException("Mekan bulunamadı.");
            if (venue.AdminUserId != adminUserId)
                throw new UnauthorizedAccessException("Bu etkinliği düzenleme yetkiniz yok.");
        }

        existing.Title           = dto.Title;
        existing.Description     = dto.Description;
        existing.Date            = dto.Date;
        existing.Capacity        = dto.Capacity;
        existing.TicketPrice     = dto.TicketPrice;
        existing.ImageUrl        = dto.ImageUrl;
        existing.TicketUrl       = dto.TicketUrl;
        existing.VenueId         = dto.VenueId;
        existing.ArtistId        = dto.ArtistId;
        existing.PromoCode       = string.IsNullOrWhiteSpace(dto.PromoCode) ? null : dto.PromoCode.ToUpperInvariant();
        existing.DiscountPercent = dto.DiscountPercent;
        existing.PromoCodeColor  = dto.PromoCodeColor;

        await _eventRepo.UpdateAsync(existing);
        _logger.LogInformation("Etkinlik güncellendi: ID {Id}", id);
    }

    public async Task DeleteAsync(int id)
    {
        await _eventRepo.DeleteAsync(id);
        _logger.LogInformation("Etkinlik silindi: ID {Id}", id);
    }

    public async Task ApproveAsync(int id)
    {
        var existing = await _eventRepo.FindWithIncludesAsync(
            e => e.Id == id, e => e.Venue, e => e.Artist);

        var ev = existing.FirstOrDefault()
            ?? throw new KeyNotFoundException("Etkinlik bulunamadı.");

        ev.IsApproved = true;
        await _eventRepo.UpdateAsync(ev);
        _logger.LogInformation("Etkinlik onaylandı: {Title} (ID: {Id})", ev.Title, id);

        var venueFavorites = await _favoriteRepo.FindAsync(f => f.VenueId == ev.VenueId);
        var artistFavorites = await _favoriteRepo.FindAsync(f => f.ArtistId == ev.ArtistId);

        var userIds = venueFavorites.Select(f => f.UserId)
            .Union(artistFavorites.Select(f => f.UserId))
            .Distinct();

        foreach (var userId in userIds)
        {
            await _notificationRepo.AddAsync(new Notification
            {
                UserId = userId,
                Message = $"Favori mekanınızda/sanatçınızda yeni etkinlik: {ev.Title} — {ev.Date:dd MMM yyyy}",
                IsRead = false,
                Type = "NewEvent"
            });
        }
    }

    public async Task RejectAsync(int id, string reason)
    {
        var ev = await _eventRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Etkinlik bulunamadı.");

        ev.IsApproved = false;
        ev.RejectionReason = reason;
        await _eventRepo.UpdateAsync(ev);
        _logger.LogInformation("Etkinlik reddedildi: ID {Id} — Sebep: {Reason}", id, reason);
    }

    private EventResponseDto MapToDto(Event e) => new()
    {
        Id = e.Id,
        VenueId = e.VenueId,
        ArtistId = e.ArtistId,
        Title = e.Title,
        Description = e.Description,
        Date = e.Date,
        TicketPrice = e.TicketPrice,
        Capacity = e.Capacity,
        SoldCount = e.SoldCount,
        IsApproved = e.IsApproved,
        ImageUrl = e.ImageUrl,
        TicketUrl = e.TicketUrl,
        VenueName       = e.Venue?.Name ?? "",
        VenueCity       = e.Venue?.City ?? "",
        ArtistName      = e.Artist?.Name ?? "",
        ArtistGenre     = e.Artist?.Genre ?? "",
        CreatedAt       = e.CreatedAt,
        PromoCode       = e.PromoCode,
        DiscountPercent = e.DiscountPercent,
        PromoCodeColor  = e.PromoCodeColor
    };
}
