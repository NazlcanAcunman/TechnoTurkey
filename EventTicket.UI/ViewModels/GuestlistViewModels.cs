using System.ComponentModel.DataAnnotations;

namespace EventTicket.UI.ViewModels;

public class GuestlistApplyViewModel
{
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Ad Soyad 3-200 karakter arasında olmalıdır.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon numarası zorunludur.")]
    [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Not en fazla 500 karakter olabilir.")]
    public string? Note { get; set; }

    public int GuestlistType { get; set; } = 0;
}

public class GuestlistRequestViewModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Note { get; set; }
    public int Status { get; set; }
    public int GuestlistType { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? AdminNote { get; set; }
    public DateTime CreatedAt { get; set; }

    public string StatusLabel => Status switch
    {
        0 => "Beklemede",
        1 => "Onaylandı",
        2 => "Reddedildi",
        _ => "Bilinmiyor"
    };

    public string GuestlistTypeLabel => GuestlistType switch
    {
        0 => "Ücretsiz",
        1 => "İndirimli",
        2 => "VIP",
        3 => "Basın",
        _ => "Diğer"
    };
}

public class GuestlistStatsViewModel
{
    public int TotalCount { get; set; }
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
}
