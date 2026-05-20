using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class PurchaseTicketDtoValidator : AbstractValidator<PurchaseTicketDto>
{
    public PurchaseTicketDtoValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("Geçerli bir etkinlik seçiniz.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("En az 1 bilet satın alınmalıdır.")
            .LessThanOrEqualTo(10).WithMessage("Bir seferde en fazla 10 bilet satın alınabilir.");

        RuleFor(x => x.SeatType)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.SeatType))
            .WithMessage("Koltuk tipi en fazla 100 karakter olabilir.");
    }
}
