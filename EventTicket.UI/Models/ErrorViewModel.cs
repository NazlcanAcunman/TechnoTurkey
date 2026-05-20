namespace EventTicket.UI.Models;

/// <summary>
/// Genel hata görünümü — /Home/Error sayfası için.
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public int StatusCode { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

/// <summary>
/// Auth işlemleri (Login / Register / Mail) sırasında oluşan hataları
/// View'a taşımak için kullanılır.
/// </summary>
public class AuthErrorViewModel
{
    /// <summary>Giriş sayfasına özgü hata (hatalı e-posta/şifre, hesap pasif vb.).</summary>
    public string? LoginError { get; set; }

    /// <summary>Kayıt sayfasına özgü hata (e-posta kullanımda, 18 yaş vb.).</summary>
    public string? RegisterError { get; set; }

    /// <summary>E-posta doğrulama veya şifre sıfırlama hatası.</summary>
    public string? MailError { get; set; }

    /// <summary>Genel bilgi mesajı (başarılı işlem sonrası).</summary>
    public string? InfoMessage { get; set; }

    public bool HasLoginError    => !string.IsNullOrEmpty(LoginError);
    public bool HasRegisterError => !string.IsNullOrEmpty(RegisterError);
    public bool HasMailError     => !string.IsNullOrEmpty(MailError);
    public bool HasInfo          => !string.IsNullOrEmpty(InfoMessage);
}
