using System.ComponentModel.DataAnnotations;

namespace EventTicket.UI.ViewModels;

public class GuestlistApplyViewModel
{
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string? EventDressCode { get; set; }

    [Required(ErrorMessage = "Misafir adı soyadı zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Ad Soyad 2-200 karakter arasında olmalıdır.")]
    public string GuestName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir.")]
    public string? GuestPhone { get; set; }

    [StringLength(500, ErrorMessage = "Not en fazla 500 karakter olabilir.")]
    public string? Note { get; set; }

    public string? Gender { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Sorumluluk reddi beyanını kabul etmelisiniz.")]
    public bool TermsAccepted { get; set; }
}

public class GuestlistRequestViewModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string AddedByUserId { get; set; } = string.Empty;
    public string AddedByUserEmail { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? Note { get; set; }
    public string? Gender { get; set; }
    public bool TermsAccepted { get; set; }
    public int Status { get; set; }
    public string? AdminNote { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public string StatusLabel => Status switch
    {
        0 => "Beklemede",
        1 => "Onaylandı",
        2 => "Reddedildi",
        _ => "Bilinmiyor"
    };
}

public class GuestlistStatsViewModel
{
    public int TotalCount { get; set; }
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
}
