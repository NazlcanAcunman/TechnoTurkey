using System;
using System.Collections.Generic;
using System.Text;
namespace EventTicket.Core.Entities;

public class Notification : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public string Type { get; set; } = string.Empty;
    public string? Link { get; set; }             
    public int? RelatedEntityId { get; set; }      
}