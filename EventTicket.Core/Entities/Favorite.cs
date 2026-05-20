using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.Entities;

public class Favorite : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int? VenueId { get; set; }
    public int? ArtistId { get; set; } 
    public Venue? Venue { get; set; }
    public Artist? Artist { get; set; }
}
