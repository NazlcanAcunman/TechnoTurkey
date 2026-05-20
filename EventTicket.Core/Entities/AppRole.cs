using Microsoft.AspNetCore.Identity;

namespace EventTicket.Core.Entities;

public class AppRole : IdentityRole
{
    public string? Description { get; set; }

    public AppRole() { }
    public AppRole(string roleName, string? description = null) : base(roleName)
    {
        Description = description;
    }
}
