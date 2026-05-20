using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.DTOs;

public class FavoriteResponseDto
{
    public int Id { get; set; }
    public int? VenueId { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public int? ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;

}
