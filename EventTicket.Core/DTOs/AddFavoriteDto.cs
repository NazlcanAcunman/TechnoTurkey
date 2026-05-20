using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class AddFavoriteDto
{
    public int? VenueId { get; set; }
    public int? ArtistId { get; set; }

}