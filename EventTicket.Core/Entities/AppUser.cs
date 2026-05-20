using Microsoft.AspNetCore.Identity;

namespace EventTicket.Core.Entities;

public class AppUser : IdentityUser
{
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
