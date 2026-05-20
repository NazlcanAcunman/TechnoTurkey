using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class ContactFormDtoValidator : AbstractValidator<ContactFormDto>
{
    public ContactFormDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad Soyad zorunludur.")
            .MaximumLength(200).WithMessage("Ad Soyad en fazla 200 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(256).WithMessage("E-posta en fazla 256 karakter olabilir.");

        RuleFor(x => x.Subject)
            .MaximumLength(500).WithMessage("Konu en fazla 500 karakter olabilir.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj zorunludur.")
            .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır.")
            .MaximumLength(5000).WithMessage("Mesaj en fazla 5000 karakter olabilir.");
    }
}
