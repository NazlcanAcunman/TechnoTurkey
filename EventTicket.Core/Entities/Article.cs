namespace EventTicket.Core.Entities;

public class Article : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Tag { get; set; } = "Haber"; // Haber, Röportaj, Festival, Mekan
    public string? TagColor { get; set; }
    public string Author { get; set; } = string.Empty;
    public int ReadTime { get; set; } = 3;
    public bool IsPublished { get; set; } = false;
}
