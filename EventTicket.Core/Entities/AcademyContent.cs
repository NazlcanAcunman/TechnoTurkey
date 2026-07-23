namespace EventTicket.Core.Entities;

public class AcademyContent : BaseEntity
{
    public string Type { get; set; } = string.Empty; // "info", "photo", "student", "video", "instructor" ...
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MediaUrl { get; set; }
    public string? LinkUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
