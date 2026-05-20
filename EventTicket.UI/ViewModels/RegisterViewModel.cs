using System.ComponentModel.DataAnnotations;

namespace EventTicket.UI.ViewModels;

public class RegisterViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [Display(Name = "Ad Soyad")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [Display(Name = "Kullanıcı Adı")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-30 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Kullanıcı adı sadece harf, rakam ve _ içerebilir.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
    [Display(Name = "TC Kimlik No")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No yalnızca rakam içermelidir.")]
    public string TcKimlikNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
    [DataType(DataType.Date)]
    [Display(Name = "Doğum Tarihi")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Parola en az 6 karakter olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Parola Tekrarı")]
    [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var minDate = DateTime.Today.AddYears(-18);
        if (DateOfBirth > minDate)
        {
            yield return new ValidationResult(
                "Bilet satın alabilmek için 18 yaşından büyük olmanız gerekmektedir.",
                new[] { nameof(DateOfBirth) });
        }
    }
}
