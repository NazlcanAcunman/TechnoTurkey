using EventTicket.UI.Services;
using EventTicket.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventTicket.UI.Controllers;

public class MagazineController : BaseController
{
    public MagazineController(IProfileUlService profileService) : base(profileService) { }

    public IActionResult Index()
    {
        ViewData["Title"] = "Magazin";
        return View();
    }

    [Microsoft.AspNetCore.Mvc.Route("Magazine/Details/{slug}")]
    public IActionResult Details(string slug)
    {
        var article = GetArticle(slug);
        if (article == null) return NotFound();
        ViewData["Title"] = article.Title + " - Magazin";
        return View(article);
    }

    private static MagazineArticleViewModel? GetArticle(string slug) => Articles.TryGetValue(slug, out var a) ? a : null;

    private static readonly Dictionary<string, MagazineArticleViewModel> Articles = new()
    {
        // ── HERO ──────────────────────────────────────────────
        ["hero-1"] = new()
        {
            Slug = "hero-1", Tag = "Özel Röportaj",
            Title = "Sonar Istanbul 2026: Elektronik Müziğin Zirve Buluşması Yaklaşıyor",
            ImageUrl = "https://cdn.bugece.co/6905f471-a658-4ca6-a185-bb307cb3ec7c",
            Author = "EventTicket Editörü", Date = "3 Nisan 2026", ReadTime = "5 dk",
            Content = """
Dünyanın en prestijli elektronik müzik festivallerinden Sonar, 10–11 Nisan 2026 tarihleri arasında
İstanbul'a geliyor. Festival direktörü EventTicket'a özel açıklamalar yaptı.

"İstanbul artık dünya elektronik müzik haritasında ayrı bir yere sahip. Sonar olarak buraya her
gelişimizde seyircinin müziğe olan aşkından büyük ilham alıyoruz" diyen direktör, bu yılki lineup'ın
önceki yılların en büyüklerinden biri olduğunu vurguladı.

Bu yılki kadro ADRIATIQUE, Recondite, Tale of Us ve Charlotte de Witte gibi dünya yıldızlarını içeriyor.
İstanbul'un tarihi siluetine karşı dans etmek için biletler EventTicket'ta hâlâ mevcut.

Sonar Istanbul, her yıl 15.000'den fazla kişiyi bir araya getiren eşsiz bir elektronik müzik deneyimi sunuyor.
"""
        },
        ["hero-2"] = new()
        {
            Slug = "hero-2", Tag = "Festival",
            Title = "I Hate Models — Disco Inferno ile İstanbul'u Yakacak",
            ImageUrl = "https://cdn.bugece.co/cfe72717-fc72-416c-a348-3531651b9f11",
            Author = "EventTicket Editörü", Date = "1 Nisan 2026", ReadTime = "3 dk",
            Content = """
Fransız techno sanatçısı I Hate Models, 4 Temmuz 2026'da İstanbul'da "Disco Inferno" temalı setiyle
sahne alacak. Karanlık ve hipnotik sounduyla underground sahnesinin vazgeçilmez ismi, bu kez özel
bir tema ile geliyor.

"Disco Inferno, hem geçmişe hem de geleceğe açılan bir kapı. Techno ve disko elementlerini bir araya
getiren bu set, İstanbul seyircisi için hazırladığım en özel setlerden biri" diyen sanatçı, sahne
tasarımı ve ışık şovunun da büyük sürprizler içereceğini belirtti.

Biletler EventTicket'ta satışa çıktı. Kapasite sınırlı — geç kalmayın.
"""
        },
        ["hero-3"] = new()
        {
            Slug = "hero-3", Tag = "Haber",
            Title = "ZAMNA x MO Homecoming: Life Park'ta Tarihi Gece",
            ImageUrl = "https://cdn.bugece.co/77724539-d7a1-4999-9e1e-50fe62c951ba",
            Author = "EventTicket Editörü", Date = "31 Mart 2026", ReadTime = "4 dk",
            Content = """
ZAMNA ve MO Homecoming iş birliği, 6–7 Haziran 2026'da İstanbul Life Park'ta gerçekleşecek.
Black Coffee, Solomun, Tale of Us ve daha onlarca dünya yıldızını bir araya getiren bu etkinlik,
yılın en büyük outdoor festivali olmaya aday.

72 saatlik müzik maratonu; three stage, camping alanları ve premium VIP bölümleriyle eşsiz bir
deneyim sunacak. Ana sahne Afro House & Deep House, ikinci sahne Melodic Techno & Minimal,
üçüncü sahne Techno & Industrial temasıyla şekillendirildi.

Biletler EventTicket'ta satışa çıktı. İlk aşama biletleri %85 oranında tükendi.
"""
        },

        // ── HABERLER ──────────────────────────────────────────
        ["news-1"] = new()
        {
            Slug = "news-1", Tag = "Röportaj",
            Title = "Ricky Martin: \"İstanbul Sahnesinde Olmak Çok Özel Bir Duygu\"",
            ImageUrl = "https://cdn.bugece.co/58302bc5-8884-4fd3-ac7f-f6cc61be5e1e",
            Author = "EventTicket Editörü", Date = "2 Nisan 2026", ReadTime = "7 dk",
            Content = """
Latin pop efsanesi Ricky Martin, 11 Temmuz 2026'daki İstanbul konserinin öncesinde EventTicket'a
özel açıklamalar yaptı. Livin' la Vida Loca'nın mimarı, Türkiye'ye olan sevgisini ve müzik yolculuğunu anlattı.

"İstanbul benim için her zaman özel bir şehir oldu. Seyircinin enerjisi, şehrin atmosferi ve kültürel
zenginliği her sahne çıkışımı unutulmaz kılıyor" diyen Martin, yeni albümünden parçaları da sahneye
taşıyacağını açıkladı.

Konser, İstanbul'un en büyük açık hava mekanlarından birinde gerçekleşecek. Biletler EventTicket'ta satışta.
"""
        },
        ["news-2"] = new()
        {
            Slug = "news-2", Tag = "Trend",
            Title = "Melodic Techno Neden Bu Kadar Popüler? ADRIATIQUE ile Konuştuk",
            ImageUrl = "https://cdn.bugece.co/85df50ee-660e-42a6-a253-9c1cb5406e08",
            Author = "EventTicket Editörü", Date = "1 Nisan 2026", ReadTime = "4 dk",
            Content = """
İsviçreli duo ADRIATIQUE, melodic deep house ve techno alanında dünya sahnelerinin en aranan
isimlerinden biri haline geldi. Ibiza'dan Berghain'e, Tokyo'dan İstanbul'a kadar her sahnede
coşkuyla karşılanan ikili, Türkiye seyircisiyle özel bağ kurduğunu söylüyor.

"Melodic techno bu kadar popüler çünkü duyguyu ve dansı buluşturuyor. Seyirci hem hissediyor hem
de hareket ediyor — bu kombinasyon benzersiz" diyen ADRIATIQUE, 9 Mayıs 2026'da Ataköy Marina
Arena'daki konser için de büyük heyecan duyduklarını belirtti.

X by ADRIATIQUE konseri için biletler EventTicket'ta mevcut.
"""
        },
        ["news-3"] = new()
        {
            Slug = "news-3", Tag = "Festival",
            Title = "INCORE Urban Festival: Wiz Khalifa İstanbul'u Sallayacak",
            ImageUrl = "https://cdn.bugece.co/ff6a5f1b-7dcf-49ff-ad9d-9b5b72ada165",
            Author = "EventTicket Editörü", Date = "30 Mart 2026", ReadTime = "3 dk",
            Content = """
Grammy ödüllü rapper Wiz Khalifa, 7 Temmuz 2026'da INCORE Urban Festival kapsamında İstanbul'a geliyor.
"See You Again" ve "Work Hard, Play Hard" gibi hit parçalarıyla milyonlarca hayrana ulaşan sanatçı,
bu kez dev bir festival sahnesinde Türkiye'yle buluşacak.

INCORE Urban Festival bu yıl üç sahne ve 20'den fazla sanatçıyla en büyük edisyonuna hazırlanıyor.
Hip-hop, R&B, trap ve electronic müziği birleştiren format, her müzik severi buluşturuyor.

Bilet talebi tüm beklentilerin üzerinde. EventTicket'ta sınırlı sayıda bilet kaldı.
"""
        },
        ["news-4"] = new()
        {
            Slug = "news-4", Tag = "Özel",
            Title = "Catwalk Presents: Charlie Sparks ile Mayıs'ta Unutulmaz Bir Gece",
            ImageUrl = "https://cdn.bugece.co/390cf8a4-064c-4f3b-a93a-891978565f81",
            Author = "EventTicket Editörü", Date = "29 Mart 2026", ReadTime = "4 dk",
            Content = """
İstanbul'un en prestijli elektronik müzik serileri arasında yer alan Catwalk, 29–30 Mayıs 2026
tarihlerinde Charlie Sparks ile geri dönüyor. Dark techno ve industrial sesler arasında şekillenen
bu gece, sinemanın kıyısında dans edenler için tasarlandı.

Catwalk serisinin kurucusu EventTicket'a konuştu: "Charlie Sparks bu formatla mükemmel uyuşuyor.
Sesin, ışığın ve mekânın birbiriyle dans ettiği bir gece olacak. İstanbul her zaman bize verdiğinden
daha fazlasını geri veriyor."

29–30 Mayıs biletleri EventTicket'ta satışta. Her iki gece için de sınırlı sayıda bilet kaldı.
"""
        },
        ["news-5"] = new()
        {
            Slug = "news-5", Tag = "Haber",
            Title = "Kevin de Vries Klein Phönix'te İzmir'i Coşturacak",
            ImageUrl = "https://cdn.bugece.co/e56b65a3-02a6-44bd-b968-7d721e51df2d",
            Author = "EventTicket Editörü", Date = "28 Mart 2026", ReadTime = "2 dk",
            Content = """
Hollandalı melodic techno ustası Kevin de Vries, 11–12 Nisan 2026'da İzmir'in yeni gözdesi
Klein Phönix'te Avis Vox, Krey ve HZR ile birlikte sahne alacak. Karanlık ve duygusal sounduyla
dünya sahnesine hızla yükselen de Vries, İzmir konserini çok özel buluyor.

"Klein Phönix'in akustiği ve enerjisi gerçekten eşsiz. İzmir seyircisi müziği ruhunun derinliklerinde
hisseden bir kitle — bu gece birlikte tarih yazacağız" diyen sanatçı, yeni setini de bu sahne için
özel olarak tasarladığını açıkladı.

Biletler EventTicket'ta satışa çıktı.
"""
        },
        ["news-6"] = new()
        {
            Slug = "news-6", Tag = "Analiz",
            Title = "2026 Yaz Festivalleri: Hangi Şehir Öne Çıkıyor?",
            ImageUrl = "https://cdn.bugece.co/2c2ce7bb-1692-4d98-9eac-40412ca4e602",
            Author = "EventTicket Editörü", Date = "27 Mart 2026", ReadTime = "8 dk",
            Content = """
Bu yaz Türkiye'de festival rekabeti kızışıyor. Bodrum, İzmir, İstanbul ve Kapadokya etkinliklerle
dolup taşarken hangi şehir öne çıkıyor?

İstanbul hem klasik konser mekanlarıyla hem de yeni açık hava alanlarıyla diğer şehirlerin önünde.
ZAMNA x MO Homecoming ve Sonar Istanbul, şehri bu yaz uluslararası müzik dünyasının merkezine
taşıyor. Kapasite: 15.000–50.000 kişi.

Bodrum ise elektronik müzik festivalleriyle benzersiz bir konuma sahip. ADRIATIQUE ve Sara Landry
gibi isimler deniz kenarında dans etmeyi bir deneyime dönüştürüyor. İzmir'de Klein Phönix,
Kevin de Vries ve Sara Landry ile güçlü bir sezon açılışı yapıyor.

Çeşme ve Alaçatı ise beach party kültürüyle bilinçli bir seyirci kitlesine hitap ediyor.
Sonuç? Bu yaz Türkiye'de her şehirde festival var — asıl soru nereye gitmeyin.
"""
        },

        // ── RÖPORTAJLAR ───────────────────────────────────────
        ["int-1"] = new()
        {
            Slug = "int-1", Tag = "Röportaj",
            Title = "Sonar Festivali Direktörü: \"İstanbul Elektronik Müziğin Yeni Kalbi\"",
            ImageUrl = "https://cdn.bugece.co/6905f471-a658-4ca6-a185-bb307cb3ec7c",
            Author = "EventTicket Editörü", Date = "26 Mart 2026", ReadTime = "6 dk",
            Content = """
Sonar Istanbul 2026 direktörü, festivalin Türkiye'deki büyümesini ve gelecek vizyonunu
EventTicket'a anlattı. Barcelona'da doğan Sonar, bugün dünyanın en seçkin elektronik müzik
festivallerinden biri konumunda.

"İstanbul artık sadece bir durak değil, bir merkez. Şehrin enerjisi, kültürel derinliği ve
seyircinin müzik bilgisi bizi her yıl daha fazla heyecanlandırıyor" diyen direktör, 2026
edisyonunun şimdiye kadarki en güçlü lineup'ına sahip olduğunu vurguladı.

Bu yılki kadro ADRIATIQUE, Charlotte de Witte, Recondite ve daha onlarca ismi barındırıyor.
İki gün, üç sahne, sonsuz elektronik müzik.
"""
        },
        ["int-2"] = new()
        {
            Slug = "int-2", Tag = "Röportaj",
            Title = "YE Live in İstanbul: Olimpiyat Stadyumunda Tarihi Konser",
            ImageUrl = "https://cdn.bugece.co/85ea084c-ace5-4980-adb5-47055f4473ab",
            Author = "EventTicket Editörü", Date = "25 Mart 2026", ReadTime = "5 dk",
            Content = """
Kanye West — sanatçı adıyla YE — 30 Mayıs 2026'da Atatürk Olimpiyat Stadyumu'nda tarihi bir
konser verecek. 60.000 kişilik kapasite için biletler yalnızca EventTicket'tan satılıyor.

Organizatörler, konser için hazırlıkların başladığını ve sahne prodüksiyonunun dünyanın en büyük
projelerinden biri olacağını açıkladı. Dev LED duvarlar, lazer şov ve özel efektlerle donatılmış
sahne, Türkiye tarihinin en büyük konser prodüksiyonu olma iddiasını taşıyor.

"YE Live in İstanbul sadece bir konser değil, bir sanat deneyimi" diyen organizatörler,
biletlerin %70'inin ilk günde tükendiğini belirtti.
"""
        },
        ["int-3"] = new()
        {
            Slug = "int-3", Tag = "Röportaj",
            Title = "Black Coffee ile Sohbet: \"İstanbul Benim İkinci Evim\"",
            ImageUrl = "https://cdn.bugece.co/77724539-d7a1-4999-9e1e-50fe62c951ba",
            Author = "EventTicket Editörü", Date = "24 Mart 2026", ReadTime = "9 dk",
            Content = """
Afro house'un dünya çapındaki yüzü Black Coffee, ZAMNA x MO Homecoming öncesinde Boğaz
kenarında EventTicket'la buluştu. Güney Afrikalı DJ/prodüktör, İstanbul'a olan sevgisini
ve müzik felsefesini paylaştı.

"İstanbul'da çalmak her seferinde farklı. Seyircinin ritmi hissedip özgürce dans etme biçimi
dünyada başka hiçbir yerde görmediğim bir şey. Bu şehir gerçekten benim ikinci evim oldu"
diyen Black Coffee, 6–7 Haziran'daki Life Park setini çok özel hazırladığını anlattı.

Yıllar içinde Ibiza, Glastonbury ve Coachella gibi festivallerde sahne alan sanatçı, Türkiye
seyircisiyle kurduğu bağı her şeyden değerli buluyor.
"""
        },

        // ── MEKAN / FESTİVAL ──────────────────────────────────
        ["venue-1"] = new()
        {
            Slug = "venue-1", Tag = "Mekan",
            Title = "Alaçatı'nın En İyi Açık Hava Mekanları: Yaz Rotanı Belirle",
            ImageUrl = "https://cdn.bugece.co/2c2ce7bb-1692-4d98-9eac-40412ca4e602",
            Author = "EventTicket Editörü", Date = "23 Mart 2026", ReadTime = "5 dk",
            Content = """
Çeşme ve Alaçatı, her yaz Türkiye'nin en çekici müzik destinasyonlarına dönüşüyor.
Rüzgârın dans ettiği bağ sokaklarında, gün batımı eşliğinde müzik dinlemek için en iyi
mekanları derledik.

Bu yaz Sara Landry ve I Hate Models gibi uluslararası isimler Alaçatı'nın açık hava
sahnesine konuk olacak. Sarp sokak aralarında kurulan pop-up mekanlar, tarihi taş yapılarla
harmanlanan atmosfer ve Ege'nin serinliği — ideal müzik tatilinin tarifi bu.

EventTicket üzerinden Çeşme bölgesi biletlerine ulaşabilirsiniz. Erken rezervasyon önerilir.
"""
        },
        ["fest-1"] = new()
        {
            Slug = "fest-1", Tag = "Festival",
            Title = "Sonar Istanbul 2026: Elektronik Müziğin Türkiye Buluşması",
            ImageUrl = "https://cdn.bugece.co/6905f471-a658-4ca6-a185-bb307cb3ec7c",
            Author = "EventTicket Editörü", Date = "15 Mart 2026", ReadTime = "7 dk",
            Content = """
10–11 Nisan 2026'da İstanbul'da gerçekleşecek Sonar, bu yıl önceki edisyonlarının çok ötesine
geçen bir lineup ile geliyor. Dünyanın dört bir yanından seçilmiş sanatçılar, iki gün boyunca
İstanbul'u elektronik müziğin başkentine dönüştürecek.

Festivalin gündüz ve gece ayrımı, farklı ses dünyalarına kapı aralıyor: SonarDay'de daha
deneysel ve sanatsal setler yer alırken, SonarNight'ta dans müziğinin zirvesi yaşanıyor.

Biletler EventTicket'ta. Tek gün ve iki günlük paketler mevcut. Konaklama paketleri için
festival web sitesini ziyaret edin.
"""
        },
        ["fest-2"] = new()
        {
            Slug = "fest-2", Tag = "Festival",
            Title = "Klein Phönix İzmir'in Yeni Müzik Kalesi mi?",
            ImageUrl = "https://cdn.bugece.co/e56b65a3-02a6-44bd-b968-7d721e51df2d",
            Author = "EventTicket Editörü", Date = "14 Mart 2026", ReadTime = "5 dk",
            Content = """
Kevin de Vries, Sara Landry, Avis Vox, Krey ve HZR gibi uluslararası isimler — bunların hepsi
2026'da İzmir'deki Klein Phönix sahnesinde. Şehrin elektronik müzik haritasına yeni kazınan
bu mekan, kısa sürede Türkiye'nin en çok konuşulan kulüplerinden biri oldu.

Klein Phönix'in ses sistemi ve tasarımı, Avrupa'nın en prestijli kulüplerinden ilham alarak
oluşturulmuş. Kapalı ve açık hava bölümleriyle her iklim koşuluna uygun yapısı, İzmir'e
dört mevsim canlı müzik deneyimi sunuyor.

Bu yaz Klein Phönix'teki etkinlikler için biletler EventTicket'ta satışa çıktı.
"""
        },
    };
}
