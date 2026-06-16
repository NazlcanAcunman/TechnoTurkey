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

        if (!ev.IsGuestlistOpen)
            return DataResult<GuestlistRequestResponseDto>.Fail("Bu etkinliğin guestlist başvurusu kapalı.");

        if (ev.GuestlistDeadline.HasValue && ev.GuestlistDeadline.Value <= DateTime.UtcNow)
            return DataResult<GuestlistRequestResponseDto>.Fail("Guestlist başvuru süresi dolmuştur.");

        var alreadyExists = await _guestlistRepo.AnyAsync(
            g => g.EventId == dto.EventId && g.AddedByUserId == userId
              && g.GuestName == dto.GuestName && !g.IsDeleted);

        if (alreadyExists)
            return DataResult<GuestlistRequestResponseDto>.Fail($"'{dto.GuestName}' bu etkinlik için zaten listede.");

        var request = new GuestlistRequest
        {
            EventId       = dto.EventId,
            AddedByUserId = userId,
            GuestName     = dto.GuestName,
            GuestPhone    = dto.GuestPhone,
            Note          = dto.Note,
            Gender        = dto.Gender,
            TermsAccepted = dto.TermsAccepted,
            Status        = GuestlistStatus.Pending
        };

        await _guestlistRepo.AddAsync(request);

        request.Event = ev;

        return DataResult<GuestlistRequestResponseDto>.Ok(MapToDto(request, ev.Title, userId));
    }

    public async Task<DataResult<List<GuestlistRequestResponseDto>>> CreateBulkAsync(List<CreateGuestlistRequestDto> dtos, string userId)
    {
        var results = new List<GuestlistRequestResponseDto>();

        foreach (var dto in dtos)
        {
            var result = await CreateAsync(dto, userId);
            if (result.Success && result.Data != null)
                results.Add(result.Data);
        }

        return DataResult<List<GuestlistRequestResponseDto>>.Ok(results);
    }

    public async Task<DataResult<List<GuestlistRequestResponseDto>>> GetByEventAsync(int eventId)
    {
        var requests = await _guestlistRepo.FindWithIncludesAsync(
            g => g.EventId == eventId && !g.IsDeleted,
            g => g.Event,
            g => g.AddedByUser);

        var result = requests
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => MapToDto(g, g.Event.Title, g.AddedByUser.Email ?? string.Empty))
            .ToList();

        return DataResult<List<GuestlistRequestResponseDto>>.Ok(result);
    }

    public async Task<DataResult<List<GuestlistRequestResponseDto>>> GetByUserAsync(string userId)
    {
        var requests = await _guestlistRepo.FindWithIncludesAsync(
            g => g.AddedByUserId == userId && !g.IsDeleted,
            g => g.Event,
            g => g.AddedByUser);

        var result = requests
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => MapToDto(g, g.Event.Title, g.AddedByUser.Email ?? string.Empty))
            .ToList();

        return DataResult<List<GuestlistRequestResponseDto>>.Ok(result);
    }

    public async Task<DataResult<GuestlistRequestResponseDto>> UpdateStatusAsync(int id, UpdateGuestlistStatusDto dto)
    {
        var requests = await _guestlistRepo.FindWithIncludesAsync(
            g => g.Id == id && !g.IsDeleted,
            g => g.Event,
            g => g.AddedByUser);

        var request = requests.FirstOrDefault();
        if (request == null)
            return DataResult<GuestlistRequestResponseDto>.Fail("Kayıt bulunamadı.");

        request.Status    = dto.Status;
        request.AdminNote = dto.AdminNote;

        if (dto.Status == GuestlistStatus.Approved)
            request.ApprovedAt = DateTime.UtcNow;
        else if (dto.Status == GuestlistStatus.Rejected)
            request.RejectedAt = DateTime.UtcNow;

        await _guestlistRepo.UpdateAsync(request);

        return DataResult<GuestlistRequestResponseDto>.Ok(
            MapToDto(request, request.Event.Title, request.AddedByUser.Email ?? string.Empty));
    }

    public async Task<DataResult<GuestlistStatsDto>> GetStatsAsync(int eventId)
    {
        var all  = await _guestlistRepo.FindAsync(g => g.EventId == eventId && !g.IsDeleted);
        var list = all.ToList();

        var stats = new GuestlistStatsDto
        {
            TotalCount    = list.Count,
            PendingCount  = list.Count(g => g.Status == GuestlistStatus.Pending),
            ApprovedCount = list.Count(g => g.Status == GuestlistStatus.Approved),
            RejectedCount = list.Count(g => g.Status == GuestlistStatus.Rejected)
        };

        return DataResult<GuestlistStatsDto>.Ok(stats);
    }

    public async Task<DataResult<bool>> DeleteAsync(int id)
    {
        var request = await _guestlistRepo.GetByIdAsync(id);
        if (request == null)
            return DataResult<bool>.Fail("Kayıt bulunamadı.");

        request.IsDeleted = true;
        await _guestlistRepo.UpdateAsync(request);

        return DataResult<bool>.Ok(true, "Kayıt silindi.");
    }

    private static GuestlistRequestResponseDto MapToDto(GuestlistRequest g, string eventTitle, string addedByUserEmail) => new()
    {
        Id               = g.Id,
        EventId          = g.EventId,
        EventTitle       = eventTitle,
        AddedByUserId    = g.AddedByUserId,
        AddedByUserEmail = addedByUserEmail,
        GuestName        = g.GuestName,
        GuestPhone       = g.GuestPhone,
        Note             = g.Note,
        Gender           = g.Gender,
        TermsAccepted    = g.TermsAccepted,
        Status           = g.Status,
        ApprovedAt       = g.ApprovedAt,
        RejectedAt       = g.RejectedAt,
        AdminNote        = g.AdminNote,
        CreatedAt        = g.CreatedAt
    };
}
