namespace EventTicket.Core.DTOs;

public class SendNotificationDto
{
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
}
