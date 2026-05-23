using EventTicket.Core.DTOs;

namespace EventTicket.Core.Interfaces;

public interface IArticleService
{
    Task<IEnumerable<ArticleResponseDto>> GetPublishedAsync();
    Task<IEnumerable<ArticleResponseDto>> GetAllAsync();
    Task<ArticleResponseDto?> GetByIdAsync(int id);
    Task<ArticleResponseDto?> GetBySlugAsync(string slug);
    Task<ArticleResponseDto> CreateAsync(CreateArticleDto dto);
    Task UpdateAsync(int id, UpdateArticleDto dto);
    Task DeleteAsync(int id);
}
