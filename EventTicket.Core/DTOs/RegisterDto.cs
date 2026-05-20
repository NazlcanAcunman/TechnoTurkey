using System.ComponentModel.DataAnnotations;

namespace EventTicket.Core.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(30, ErrorMessage = "Kullanıcı adı en fazla 30 karakter olabilir.")]
    public string UserName { get; set; } = string.Empty;



    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No yalnızca rakam içermelidir.")]
    public string TcKimlikNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
    public DateTime DateOfBirth { get; set; }
}