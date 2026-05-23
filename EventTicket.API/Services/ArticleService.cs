using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class ArticleService : IArticleService
{
    private readonly IRepository<Article> _repo;

    public ArticleService(IRepository<Article> repo) => _repo = repo;

    public async Task<IEnumerable<ArticleResponseDto>> GetPublishedAsync()
    {
        var articles = await _repo.FindAsync(a => a.IsPublished && !a.IsDeleted);
        return articles.OrderByDescending(a => a.CreatedAt).Select(Map);
    }

    public async Task<IEnumerable<ArticleResponseDto>> GetAllAsync()
    {
        var articles = await _repo.FindAsync(a => !a.IsDeleted);
        return articles.OrderByDescending(a => a.CreatedAt).Select(Map);
    }

    public async Task<ArticleResponseDto?> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        return a == null || a.IsDeleted ? null : Map(a);
    }

    public async Task<ArticleResponseDto?> GetBySlugAsync(string slug)
    {
        var articles = await _repo.FindAsync(a => a.Slug == slug && !a.IsDeleted);
        var a = articles.FirstOrDefault();
        return a == null ? null : Map(a);
    }

    public async Task<ArticleResponseDto> CreateAsync(CreateArticleDto dto)
    {
        var slug = GenerateSlug(dto.Title);
        var article = new Article
        {
            Title = dto.Title,
            Slug = slug,
            Summary = dto.Summary,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            Tag = dto.Tag,
            Author = dto.Author,
            ReadTime = dto.ReadTime,
            IsPublished = dto.IsPublished
        };
        await _repo.AddAsync(article);
        return Map(article);
    }

    public async Task UpdateAsync(int id, UpdateArticleDto dto)
    {
        var article = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Makale bulunamadı.");

        article.Title = dto.Title;
        article.Slug = GenerateSlug(dto.Title);
        article.Summary = dto.Summary;
        article.Content = dto.Content;
        article.ImageUrl = dto.ImageUrl;
        article.Tag = dto.Tag;
        article.Author = dto.Author;
        article.ReadTime = dto.ReadTime;
        article.IsPublished = dto.IsPublished;

        await _repo.UpdateAsync(article);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }

    private static string GenerateSlug(string title)
    {
        var slug = title.ToLowerInvariant()
            .Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u")
            .Replace("ş", "s").Replace("ö", "o").Replace("ç", "c")
            .Replace(" ", "-");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-").Trim('-');
        return slug + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    private static ArticleResponseDto Map(Article a) => new()
    {
        Id = a.Id,
        Title = a.Title,
        Slug = a.Slug,
        Summary = a.Summary,
        Content = a.Content,
        ImageUrl = a.ImageUrl,
        Tag = a.Tag,
        Author = a.Author,
        ReadTime = a.ReadTime,
        IsPublished = a.IsPublished,
        CreatedAt = a.CreatedAt
    };
}
