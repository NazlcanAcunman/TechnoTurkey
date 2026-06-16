using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class CreateGuestlistRequestDtoValidator : AbstractValidator<CreateGuestlistRequestDto>
{
    public CreateGuestlistRequestDtoValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("Geçerli bir etkinlik belirtilmelidir.");

        RuleFor(x => x.GuestName)
            .NotEmpty().WithMessage("Misafir adı zorunludur.")
            .MinimumLength(3).WithMessage("Misafir adı en az 3 karakter olmalıdır.")
            .MaximumLength(200).WithMessage("Misafir adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.GuestPhone)
            .Matches(@"^[\d\s\+\-\(\)]{7,20}$").WithMessage("Geçerli bir telefon numarası giriniz.")
            .When(x => !string.IsNullOrEmpty(x.GuestPhone));

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Not en fazla 500 karakter olabilir.")
            .When(x => x.Note != null);

        RuleFor(x => x.Gender)
            .Must(g => g == "Erkek" || g == "Kadın" || g == "Belirtmek İstemiyorum")
            .WithMessage("Geçerli bir cinsiyet seçiniz.")
            .When(x => x.Gender != null);

        RuleFor(x => x.TermsAccepted)
            .Must(x => x).WithMessage("Sorumluluk reddi beyanını kabul etmelisiniz.");
    }
}
