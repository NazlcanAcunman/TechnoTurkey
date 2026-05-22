using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class BannerService : IBannerService
{
    private readonly IRepository<Banner> _repo;

    public BannerService(IRepository<Banner> repo) => _repo = repo;

    public async Task<IEnumerable<BannerResponseDto>> GetActiveAsync()
    {
        var banners = await _repo.FindAsync(b => b.IsActive && !b.IsDeleted);
        return banners.OrderBy(b => b.SortOrder).Select(Map);
    }

    public async Task<IEnumerable<BannerResponseDto>> GetAllAsync()
    {
        var banners = await _repo.FindAsync(b => !b.IsDeleted);
        return banners.OrderBy(b => b.SortOrder).Select(Map);
    }

    public async Task<BannerResponseDto?> GetByIdAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        return b == null || b.IsDeleted ? null : Map(b);
    }

    public async Task<BannerResponseDto> CreateAsync(CreateBannerDto dto)
    {
        var b = new Banner
        {
            Title = dto.Title,
            SubTitle = dto.SubTitle,
            DateText = dto.DateText,
            Venue = dto.Venue,
            ImageUrl = dto.ImageUrl,
            SearchQuery = dto.SearchQuery,
            TicketUrl = dto.TicketUrl,
            Type = dto.Type,
            SortOrder = dto.SortOrder,
            IsActive = dto.IsActive,
            TagText = dto.TagText,
            TextColor = dto.TextColor,
            BgColor = dto.BgColor
        };
        await _repo.AddAsync(b);
        return Map(b);
    }

    public async Task UpdateAsync(int id, UpdateBannerDto dto)
    {
        var b = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Banner bulunamadı.");
        b.Title = dto.Title;
        b.SubTitle = dto.SubTitle;
        b.DateText = dto.DateText;
        b.Venue = dto.Venue;
        b.ImageUrl = dto.ImageUrl;
        b.SearchQuery = dto.SearchQuery;
        b.TicketUrl = dto.TicketUrl;
        b.Type = dto.Type;
        b.SortOrder = dto.SortOrder;
        b.IsActive = dto.IsActive;
        b.TagText = dto.TagText;
        b.TextColor = dto.TextColor;
        b.BgColor = dto.BgColor;
        await _repo.UpdateAsync(b);
    }

    public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static BannerResponseDto Map(Banner b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        SubTitle = b.SubTitle,
        DateText = b.DateText,
        Venue = b.Venue,
        ImageUrl = b.ImageUrl,
        SearchQuery = b.SearchQuery,
        TicketUrl = b.TicketUrl,
        Type = b.Type,
        SortOrder = b.SortOrder,
        IsActive = b.IsActive,
        TagText = b.TagText,
        TextColor = b.TextColor,
        BgColor = b.BgColor
    };
}
