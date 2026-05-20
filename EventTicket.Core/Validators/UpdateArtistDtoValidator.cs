using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class UpdateArtistDtoValidator : AbstractValidator<UpdateArtistDto>
{
    public UpdateArtistDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Sanatçı adı zorunludur.")
            .MaximumLength(200).WithMessage("Sanatçı adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Bio)
            .MaximumLength(2000).WithMessage("Biyografi en fazla 2000 karakter olabilir.");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Tür zorunludur.")
            .MaximumLength(100).WithMessage("Tür en fazla 100 karakter olabilir.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Görsel URL en fazla 500 karakter olabilir.")
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Geçerli bir URL giriniz.");
    }
}
