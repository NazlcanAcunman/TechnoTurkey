using EventTicket.Core.DTOs;
using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;

namespace EventTicket.API.Services;

public class TicketService : ITicketService
{
    private readonly IUnitOfWork _uow;

    public TicketService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<TicketResponseDto>> PurchaseAsync(PurchaseTicketDto dto, string userId)
    {
        var eventRepo = _uow.Repository<Event>();
        var orderRepo = _uow.Repository<Order>();
        var ticketRepo = _uow.Repository<Ticket>();

        var existingEvent = await eventRepo.GetByIdAsync(dto.EventId)
            ?? throw new KeyNotFoundException("Etkinlik bulunamadı.");

        if (!existingEvent.IsApproved)
            throw new InvalidOperationException("Bu etkinlik henüz onaylanmamış.");

        if (existingEvent.SoldCount + dto.Quantity > existingEvent.Capacity)
            throw new InvalidOperationException("Yeterli bilet kalmadı.");

        await _uow.BeginTransactionAsync();
        try
        {
            var order = new Order
            {
                UserId = userId,
                TotalAmount = existingEvent.TicketPrice * dto.Quantity,
                PaymentStatus = "Tamamlandı"
            };
            await orderRepo.AddAsync(order);

            var tickets = new List<Ticket>();
            for (int i = 0; i < dto.Quantity; i++)
            {
                var ticket = new Ticket
                {
                    UserId = userId,
                    EventId = dto.EventId,
                    OrderId = order.Id,
                    QRCode = Guid.NewGuid().ToString(),
                    SeatInfo = dto.SeatType ?? "Genel"
                };
                await ticketRepo.AddAsync(ticket);
                tickets.Add(ticket);
            }

            existingEvent.SoldCount += dto.Quantity;
            await eventRepo.UpdateAsync(existingEvent);
            await _uow.CommitTransactionAsync();

            var venueRepo = _uow.Repository<Venue>();
            var venue = await venueRepo.GetByIdAsync(existingEvent.VenueId);

            return tickets.Select(t => new TicketResponseDto
            {
                Id = t.Id,
                EventId = t.EventId,
                OrderId = t.OrderId,
                EventTitle = existingEvent.Title,
                EventDate = existingEvent.Date,
                VenueName = venue?.Name ?? "",
                VenueCity = venue?.City ?? "",
                TicketPrice = existingEvent.TicketPrice,
                QRCode = t.QRCode,
                SeatInfo = t.SeatInfo,
                PurchaseDate = t.CreatedAt
            });
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(string userId)
    {
        var ticketRepo = _uow.Repository<Ticket>();
        var tickets = await ticketRepo.FindWithIncludesAsync(
            t => t.UserId == userId,
            t => t.Event,
            t => t.Order);
        return tickets.Select(MapToDto);
    }

    public async Task<TicketResponseDto?> GetByIdAsync(int id)
    {
        var ticketRepo = _uow.Repository<Ticket>();
        var tickets = await ticketRepo.FindWithIncludesAsync(
            t => t.Id == id,
            t => t.Event,
            t => t.Order);
        var ticket = tickets.FirstOrDefault();
        return ticket == null ? null : MapToDto(ticket);
    }

    private TicketResponseDto MapToDto(Ticket t) => new()
    {
        Id = t.Id,
        EventId = t.EventId,
        OrderId = t.OrderId,
        EventTitle = t.Event?.Title ?? "",
        ArtistName = t.Event?.Artist?.Name ?? "",
        EventDate = t.Event?.Date ?? DateTime.MinValue,
        VenueName = t.Event?.Venue?.Name ?? "",
        VenueCity = t.Event?.Venue?.City ?? "",
        TicketPrice = t.Event?.TicketPrice ?? 0,
        QRCode = t.QRCode,
        SeatInfo = t.SeatInfo,
        PurchaseDate = t.CreatedAt
    };
}