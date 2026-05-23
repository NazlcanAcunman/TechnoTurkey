namespace EventTicket.Core.DTOs;

public class ArticleResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string? TagColor { get; set; }
    public string Author { get; set; } = string.Empty;
    public int ReadTime { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Tag { get; set; } = "Haber";
    public string? TagColor { get; set; }
    public string Author { get; set; } = string.Empty;
    public int ReadTime { get; set; } = 3;
    public bool IsPublished { get; set; } = false;
}

public class UpdateArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Tag { get; set; } = "Haber";
    public string? TagColor { get; set; }
    public string Author { get; set; } = string.Empty;
    public int ReadTime { get; set; } = 3;
    public bool IsPublished { get; set; }
}
