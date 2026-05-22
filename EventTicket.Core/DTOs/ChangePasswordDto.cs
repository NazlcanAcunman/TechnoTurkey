using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}

public class UpdateUsernameDto
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_]+$")]
    public string Username { get; set; } = string.Empty;
}
