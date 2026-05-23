using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public interface IArticleUlService
{
    Task<List<ArticleViewModel>> GetPublishedAsync();
    Task<List<ArticleViewModel>> GetAllAsync();
    Task<ArticleViewModel?> GetByIdAsync(int id);
    Task<ArticleViewModel?> GetBySlugAsync(string slug);
    Task CreateAsync(object dto);
    Task UpdateAsync(int id, object dto);
    Task DeleteAsync(int id);
}
