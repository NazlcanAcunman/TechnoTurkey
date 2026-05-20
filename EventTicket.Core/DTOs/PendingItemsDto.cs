namespace EventTicket.Core.DTOs;

public class PendingItemsDto
{
    public IEnumerable<EventResponseDto> Events { get; set; } = [];
    public IEnumerable<VenueResponseDto> Venues { get; set; } = [];
    public IEnumerable<ArtistResponseDto> Artists { get; set; } = [];
}