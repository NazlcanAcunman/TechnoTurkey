using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Etkinlik adı zorunludur.")
            .MaximumLength(200).WithMessage("Etkinlik adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir."); 

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.UtcNow).WithMessage("Etkinlik tarihi gelecekte olmalıdır.")
            .LessThan(DateTime.UtcNow.AddYears(5)).WithMessage("Etkinlik tarihi 5 yıldan fazla ileri olamaz."); 

        RuleFor(x => x.ImageUrl)                                                           
            .MaximumLength(500).WithMessage("Görsel URL en fazla 500 karakter olabilir.")
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Geçerli bir URL giriniz.");

        RuleFor(x => x.VenueId)
            .GreaterThan(0).WithMessage("Mekan seçimi zorunludur.");

        RuleFor(x => x.ArtistId)
            .GreaterThan(0).WithMessage("Sanatçı seçimi zorunludur.");
    }
}