using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.Entities;

public class Artist  : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
