using EventTicket.Core.DTOs;
using FluentValidation;

namespace EventTicket.Core.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Kullanıcı adı zorunludur.")
            .MinimumLength(2).WithMessage("Kullanıcı adı en az 2 karakter olmalıdır.")
            .MaximumLength(30).WithMessage("Kullanıcı adı en fazla 30 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(256).WithMessage("E-posta en fazla 256 karakter olabilir."); 

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre zorunludur.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .Matches(@"\d").WithMessage("Şifre en az bir rakam içermelidir.")      
            .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir."); 

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad zorunludur.")
            .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.TcKimlikNo)
            .NotEmpty().WithMessage("TC Kimlik No zorunludur.")
            .Length(11).WithMessage("TC Kimlik No 11 haneli olmalıdır.")
            .Matches(@"^\d{11}$").WithMessage("TC Kimlik No yalnızca rakam içermelidir.")
            .Must(tc => string.IsNullOrEmpty(tc) || tc[0] != '0').WithMessage("TC Kimlik No 0 ile başlayamaz.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Doğum tarihi zorunludur.")
            .LessThan(DateTime.Today).WithMessage("Doğum tarihi bugünden önce olmalıdır.") 
            .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("Geçerli bir doğum tarihi giriniz.") 
            .Must(dob => dob <= DateTime.Today.AddYears(-18))
            .WithMessage("Kayıt olabilmek için 18 yaşından büyük olmanız gerekmektedir.");
    }
}