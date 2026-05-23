using EventTicket.UI.ViewModels;

namespace EventTicket.UI.Services;

public class ArticleUlService : IArticleUlService
{
    private readonly IApiService _api;

    public ArticleUlService(IApiService api) => _api = api;

    public async Task<List<ArticleViewModel>> GetPublishedAsync()
        => await _api.GetAsync<List<ArticleViewModel>>("api/articles") ?? new();

    public async Task<List<ArticleViewModel>> GetAllAsync()
        => await _api.GetAsync<List<ArticleViewModel>>("api/articles/all") ?? new();

    public async Task<ArticleViewModel?> GetByIdAsync(int id)
        => await _api.GetAsync<ArticleViewModel>($"api/articles/{id}");

    public async Task<ArticleViewModel?> GetBySlugAsync(string slug)
        => await _api.GetAsync<ArticleViewModel>($"api/articles/slug/{slug}");

    public async Task CreateAsync(object dto)
        => await _api.PostAsync<object>("api/articles", dto);

    public async Task UpdateAsync(int id, object dto)
        => await _api.PutAsync($"api/articles/{id}", dto);

    public async Task DeleteAsync(int id)
        => await _api.DeleteAsync($"api/articles/{id}");
}
