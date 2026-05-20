using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class UpdateVenueDtoValidator : AbstractValidator<UpdateVenueDto>
{
    public UpdateVenueDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Mekan adı zorunludur.")
            .MaximumLength(200).WithMessage("Mekan adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Şehir zorunludur.")
            .MaximumLength(100).WithMessage("Şehir adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adres zorunludur.")
            .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olabilir.");
    }
}
