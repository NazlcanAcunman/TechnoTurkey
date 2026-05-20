using EventTicket.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
namespace EventTicket.Core.Entities;

public abstract class BaseEntity : IEntityTimestamps
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}