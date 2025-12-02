using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

// API den gelen JSON verilerinin sınıfı 
public class Tatil
{
    [JsonPropertyName("date")]
    public string Tarih { get; set; } = string.Empty;

    [JsonPropertyName("localName")]
    public string YerelAd { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string UluslararasiAd { get; set; } = string.Empty;

    [JsonPropertyName("countryCode")]
    public string UlkeKodu { get; set; } = string.Empty;

    [JsonPropertyName("fixed")]
    public bool Sabit { get; set; }

    [JsonPropertyName("global")]
    public bool Genel { get; set; }

    // Konsolda çıktı vermek için
    public override string ToString()
    {
        return $"Tarih: {Tarih,-12} --> Yerel Ad: {YerelAd,-40} --> Uluslararası Ad: {UluslararasiAd,-30}";
    }
}

// Ana Program Sınıfı
public class TatilTakipci
{
    // HttpClient ile API nin çağrısını yapacağız.
    private static readonly HttpClient istemci = new HttpClient();

    // API'den çekilen tüm tatilleri buradaki listede tutacağız.
    private static List<Tatil> tumTatiller = new List<Tatil>();

    // API çağrısı yapılacak yıllar.
    private static readonly int[] desteklenenYillar = { 2023, 2024, 2025 };

    // API adresi şablonu. {0} yerine yıl gelecek.
    private const string ApiAdresiSablon = "https://date.nager.at/api/v3/PublicHolidays/{0}/TR";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("TÜRKİYE CUMHURİYETİ Resmi Tatil Takip Uygulaması Başlatılıyor...");

        // Uygulama başlarken verileri belleğe yükle
        await TumTatilleriYukle(desteklenenYillar);

        if (tumTatiller.Count == 0)
        {
            Console.WriteLine("Tatil verileri yüklenemedi. Uygulama sonlandırılıyor.");
            return;
        }

        Console.WriteLine($"Toplam {tumTatiller.Count} adet resmi tatil verisi yüklendi!");

        // Ana menüyü calistir
        await MenuyuCalistir();
    }

    // - API Veri Çekme -

    // Belirtilen yıllar için API'den tatilleri çeker ve listeye ekler.
    private static async Task TumTatilleriYukle(int[] yillar)
    {
        tumTatiller.Clear();
        foreach (var yil in yillar)
        {
            try
            {
                var url = string.Format(ApiAdresiSablon, yil);
                var yanit = await istemci.GetAsync(url);

                if (yanit.IsSuccessStatusCode)
                {
                    var jsonString = await yanit.Content.ReadAsStringAsync();

                    // JSON verisini Tatil listesine dönüştür.
                    var tatiller = JsonSerializer.Deserialize<List<Tatil>>(jsonString);

                    if (tatiller != null)
                    {
                        tumTatiller.AddRange(tatiller);
                    }
                }
            }// Hata kontrol bloğu.
            catch (Exception ex)
            {
                Console.WriteLine($"{yil} yılı verisi çekilirken hata oluştu: {ex.Message}");
            }
        }
    }

    // - Menü ve Kullanıcı İşlemleri Metotları -
         
    // --Ana menüyü gösterir ve kullanıcı seçimini yönetir --
    private static async Task MenuyuCalistir()
    {
        bool calisiyor = true;
        while (calisiyor)
        {
            // Ufak menü iyileştirmesi
            Console.WriteLine("\n--- TÜRKİYE'deki Resmi Tatiller ---");
            Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
            Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
            Console.WriteLine("3. İsme göre tatil ara");
            Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2024-2025)");
            Console.WriteLine("5. Çıkış");
            Console.WriteLine("------------------------------");
            Console.Write("Seçiminiz : ");

            var girdi = Console.ReadLine();
            if (int.TryParse(girdi, out int secim))
            {
                // Konsol seçimi.
                switch (secim)
                {
                    case 1:
                        YilaGoreTatilGoster();
                        break;
                    case 2:
                        TariheGoreTatilAra();
                        break;
                    case 3:
                        IsmeGoreTatilAra();
                        break;
                    case 4:
                        TumTatilleriGoster();
                        break;
                    case 5:
                        calisiyor = false;
                        Console.WriteLine("Uygulama kapatılıyor.");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen 1 ile 5 arasında bir sayı girin.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Hatalı giriş. Lütfen bir sayı giriniz.");
            }

            // Çıkış seçeneği haricinde her işlemden sonra beklet.
            if (calisiyor)
            {
                Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                Console.ReadKey();
            }
        }
    }

    // Menü 1: Yıla göre tatil listeleme.
    private static void YilaGoreTatilGoster()
    {
        Console.WriteLine("\n--- Yıla Göre Listele ---");
        Console.Write("Lütfen yılı girin (2023, 2024, 2025): ");
        var yilGirdisi = Console.ReadLine();

        if (int.TryParse(yilGirdisi, out int secilenYil) && desteklenenYillar.Contains(secilenYil))
        {
            //  Tarih 'YYYY-MM-DD' formatında olduğu için yıl ile başlayanları seç.
            var filtrelenmisTatiller = tumTatiller
                .Where(t => t.Tarih.StartsWith(secilenYil.ToString()))
                .ToList();

            if (filtrelenmisTatiller.Any())
            {
                Console.WriteLine($"\n** {secilenYil} Yılı Resmi Tatilleri **");
                foreach (var tatil in filtrelenmisTatiller)
                {
                    Console.WriteLine(tatil);
                }
                Console.WriteLine($"Toplam {filtrelenmisTatiller.Count} tatil bulundu.");
            }
            else
            {
                Console.WriteLine($"{secilenYil} yılına ait tatil bulunamadı.");
            }
        }
        else
        {
            Console.WriteLine("Geçersiz yıl. Lütfen 2023, 2024 veya 2025 girin.");
        }
    }

    // Menü 2: Tarihe (gg-aa) göre tatil arama.
    private static void TariheGoreTatilAra()
    {
        Console.WriteLine("\n--- Tarihe (gg-aa) Göre Ara ---");
        Console.Write("Aramak istediğiniz tarihi gg-aa formatında girin (örn: 23-04): ");
        var tarihGirdisi = Console.ReadLine();

        // Kullanıcı girdisini 'MM-DD' formatına çevir.
        string ayGunArama = string.Empty;
        try
        {
            if (DateTime.TryParseExact(tarihGirdisi, "dd-MM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime ayrilmisTarih))
            {
                // API tarihi 'YYYY-MM-DD' olduğu için son 6 karakteri ('-MM-DD') kontrol et.
                ayGunArama = $"-{ayrilmisTarih.ToString("MM-dd")}";
            }
            else
            {
                Console.WriteLine("Geçersiz format. gg-aa formatını kullanın (örn: 23-04).");
                return;
            }
        }
        catch { return; }

               // Tarih alanının sonu aranan ay-gün ile bitenleri bul.
        var bulunanTatiller = tumTatiller
            .Where(t => t.Tarih.EndsWith(ayGunArama))
            .ToList();

        if (bulunanTatiller.Any())
        {
            Console.WriteLine($"\n** {tarihGirdisi} Tarihine Ait Tatiller **");
            foreach (var tatil in bulunanTatiller)
            {
                Console.WriteLine(tatil);
            }
        }
        else
        {
            Console.WriteLine($"{tarihGirdisi} tarihine ait resmi tatil veya tatiller bulunamadı.");
        }
    }

    // Menü 3: İsme göre tatil arama.
    private static void IsmeGoreTatilAra()
    {
        Console.WriteLine("\n--- İsme Göre Ara ---");
        Console.Write("Aramak istediğiniz tatil adının bir kısmını girin (örn: zafer): ");
        var adGirdisi = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(adGirdisi))
        {
            Console.WriteLine("Arama metni boş olamaz.");
            return;
        }

        // Yerel veya Uluslararası isimde arama yap.
        // Büyük/küçük harf duyarsız olmalı.
        var bulunanTatiller = tumTatiller
            .Where(t => t.YerelAd.Contains(adGirdisi, StringComparison.OrdinalIgnoreCase) ||
                        t.UluslararasiAd.Contains(adGirdisi, StringComparison.OrdinalIgnoreCase))
            .OrderBy(t => t.Tarih) // Sonuçları tarihe göre sırala
            .ToList();

        if (bulunanTatiller.Any())
        {
            Console.WriteLine($"\n** '{adGirdisi}' İçeren Tatiller **");
            foreach (var tatil in bulunanTatiller)
            {
                Console.WriteLine(tatil);
            }
            Console.WriteLine($"Toplam {bulunanTatiller.Count} tatil bulundu.");
        }
        else
        {
            Console.WriteLine($"'{adGirdisi}' ile eşleşen tatil veya tatiller bulunamadı.");
        }
    }

    // Menü 4: Tüm tatilleri (2023-2024-2025) listeleme.
    private static void TumTatilleriGoster()
    {
        Console.WriteLine("\n--- 2023-2025 Tüm Resmi Tatiller ---");

        if (tumTatiller.Any())
        {
            // Tüm listeyi tarihe göre sıralayıp göster.
            var siraliTatiller = tumTatiller.OrderBy(t => t.Tarih).ToList();

            foreach (var tatil in siraliTatiller)
            {
                Console.WriteLine(tatil);
            }
            Console.WriteLine($"\nGenel Toplam: {siraliTatiller.Count} adet tatil listelendi.");
        }
        else
        {
            Console.WriteLine("Bellekte yüklü tatil bilgisi bulunmamaktadır. Lütfen Tekrar Deneyiniz!!");
        }
    }
}