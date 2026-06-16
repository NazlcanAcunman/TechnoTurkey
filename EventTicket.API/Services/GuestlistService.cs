using EventTicket.Core.Common.Results;
using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Enums;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class GuestlistService : IGuestlistService
{
    private readonly IRepository<GuestlistRequest> _guestlistRepo;
    private readonly IRepository<Event> _eventRepo;

    public GuestlistService(
        IRepository<GuestlistRequest> guestlistRepo,
        IRepository<Event> eventRepo)
    {
        _guestlistRepo = guestlistRepo;
        _eventRepo = eventRepo;
    }

    public async Task<DataResult<GuestlistRequestResponseDto>> CreateAsync(CreateGuestlistRequestDto dto, string userId)
    {
        var ev = await _eventRepo.GetByIdAsync(dto.EventId);
        if (ev == null)
            return DataResult<GuestlistRequestResponseDto>.Fail("Etkinlik bulunamadı.");

        var alreadyExists = await _guestlistRepo.AnyAsync(
            g => g.EventId == dto.EventId && g.UserId == userId && !g.IsDeleted);

        if (alreadyExists)
            return DataResult<GuestlistRequestResponseDto>.Fail("Bu etkinliğe zaten başvurmuşsunuz.");

        var request = new GuestlistRequest
        {
            EventId = dto.EventId,
            UserId = userId,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            Note = dto.Note,
            GuestlistType = dto.GuestlistType,
            Status = GuestlistStatus.Pending
        };

        await _guestlistRepo.AddAsync(request);

        request.Event = ev;

        return DataResult<GuestlistRequestResponseDto>.Ok(MapToDto(request, ev.Title, userId));
    }

    public async Task<DataResult<List<GuestlistRequestResponseDto>>> GetByEventAsync(int eventId)
    {
        var requests = await _guestlistRepo.FindWithIncludesAsync(
            g => g.EventId == eventId && !g.IsDeleted,
            g => g.Event,
            g => g.User);

        var result = requests
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => MapToDto(g, g.Event.Title, g.User.Email ?? string.Empty))
            .ToList();

        return DataResult<List<GuestlistRequestResponseDto>>.Ok(result);
    }

    public async Task<DataResult<List<GuestlistRequestResponseDto>>> GetByUserAsync(string userId)
    {
        var requests = await _guestlistRepo.FindWithIncludesAsync(
            g => g.UserId == userId && !g.IsDeleted,
            g => g.Event,
            g => g.User);

        var result = requests
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => MapToDto(g, g.Event.Title, g.User.Email ?? string.Empty))
            .ToList();

        return DataResult<List<GuestlistRequestResponseDto>>.Ok(result);
    }

    public async Task<DataResult<GuestlistRequestResponseDto>> UpdateStatusAsync(int id, UpdateGuestlistStatusDto dto)
    {
        var requests = await _guestlistRepo.FindWithIncludesAsync(
            g => g.Id == id && !g.IsDeleted,
            g => g.Event,
            g => g.User);

        var request = requests.FirstOrDefault();
        if (request == null)
            return DataResult<GuestlistRequestResponseDto>.Fail("Başvuru bulunamadı.");

        request.Status = dto.Status;
        request.AdminNote = dto.AdminNote;

        if (dto.Status == GuestlistStatus.Approved)
            request.ApprovedAt = DateTime.UtcNow;
        else if (dto.Status == GuestlistStatus.Rejected)
            request.RejectedAt = DateTime.UtcNow;

        await _guestlistRepo.UpdateAsync(request);

        return DataResult<GuestlistRequestResponseDto>.Ok(
            MapToDto(request, request.Event.Title, request.User.Email ?? string.Empty));
    }

    public async Task<DataResult<GuestlistStatsDto>> GetStatsAsync(int eventId)
    {
        var all = await _guestlistRepo.FindAsync(g => g.EventId == eventId && !g.IsDeleted);
        var list = all.ToList();

        var stats = new GuestlistStatsDto
        {
            TotalCount = list.Count,
            PendingCount = list.Count(g => g.Status == GuestlistStatus.Pending),
            ApprovedCount = list.Count(g => g.Status == GuestlistStatus.Approved),
            RejectedCount = list.Count(g => g.Status == GuestlistStatus.Rejected)
        };

        return DataResult<GuestlistStatsDto>.Ok(stats);
    }

    public async Task<DataResult<bool>> DeleteAsync(int id)
    {
        var request = await _guestlistRepo.GetByIdAsync(id);
        if (request == null)
            return DataResult<bool>.Fail("Başvuru bulunamadı.");

        request.IsDeleted = true;
        await _guestlistRepo.UpdateAsync(request);

        return DataResult<bool>.Ok(true, "Başvuru silindi.");
    }

    private static GuestlistRequestResponseDto MapToDto(GuestlistRequest g, string eventTitle, string userEmail) => new()
    {
        Id = g.Id,
        EventId = g.EventId,
        EventTitle = eventTitle,
        UserId = g.UserId,
        UserEmail = userEmail,
        FullName = g.FullName,
        Phone = g.Phone,
        Email = g.Email,
        Note = g.Note,
        Status = g.Status,
        GuestlistType = g.GuestlistType,
        ApprovedAt = g.ApprovedAt,
        RejectedAt = g.RejectedAt,
        AdminNote = g.AdminNote,
        CreatedAt = g.CreatedAt
    };
}
