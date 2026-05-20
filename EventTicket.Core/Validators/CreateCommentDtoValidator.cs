using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Yorum içeriği zorunludur.")
            .MinimumLength(2).WithMessage("Yorum en az 2 karakter olmalıdır.")  
            .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olabilir.");

        RuleFor(x => x.Rating)
            .NotNull().WithMessage("Puan zorunludur.")                           
            .InclusiveBetween(1, 5).WithMessage("Puan 1 ile 5 arasında olmalıdır.");

       
        RuleFor(x => x)
            .Must(x => x.EventId.HasValue || x.VenueId.HasValue)
            .WithMessage("Etkinlik veya mekan belirtilmelidir.")
            .OverridePropertyName("EventId");

                                            
        RuleFor(x => x)
            .Must(x => !(x.EventId.HasValue && x.VenueId.HasValue))
            .WithMessage("Aynı anda hem etkinlik hem mekan için yorum yapılamaz.")
            .OverridePropertyName("VenueId");

        RuleFor(x => x.EventId)
            .GreaterThan(0).When(x => x.EventId.HasValue)
            .WithMessage("Geçerli bir etkinlik ID giriniz.");

        RuleFor(x => x.VenueId)
            .GreaterThan(0).When(x => x.VenueId.HasValue)
            .WithMessage("Geçerli bir mekan ID giriniz.");
    }
}
