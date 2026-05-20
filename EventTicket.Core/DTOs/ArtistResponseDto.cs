using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.DTOs;

public class ArtistResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
}
