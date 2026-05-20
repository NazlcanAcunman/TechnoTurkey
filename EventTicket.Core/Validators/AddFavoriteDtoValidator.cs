using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class AddFavoriteDtoValidator : AbstractValidator<AddFavoriteDto>
{
    public AddFavoriteDtoValidator()
    {
        RuleFor(x => x)
            .Must(x => x.VenueId.HasValue || x.ArtistId.HasValue)
            .WithMessage("Mekan veya sanatçı belirtilmelidir.")
            .OverridePropertyName("VenueId");

        RuleFor(x => x)
            .Must(x => !(x.VenueId.HasValue && x.ArtistId.HasValue))
            .WithMessage("Aynı anda hem mekan hem sanatçı favorilenеmez. Ayrı ayrı gönderin.")
            .OverridePropertyName("ArtistId");

        RuleFor(x => x.VenueId)
            .GreaterThan(0).When(x => x.VenueId.HasValue)
            .WithMessage("Geçerli bir mekan ID giriniz.");

        RuleFor(x => x.ArtistId)
            .GreaterThan(0).When(x => x.ArtistId.HasValue)
            .WithMessage("Geçerli bir sanatçı ID giriniz.");
    }
}
