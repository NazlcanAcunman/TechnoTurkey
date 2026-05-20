using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.DTOs;

public class NotificationResponseDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

}
