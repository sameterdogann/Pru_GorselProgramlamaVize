# 🇹🇷 Türkiye Cumhuriyeti Resmi Tatil Takip Uygulaması

Bu proje, Türkiye'deki resmi tatil günlerini harici bir API (`date.nager.at`) üzerinden çekip konsol ekranında listeleyen ve arama yapmaya olanak tanıyan bir **C# konsol uygulamasıdır**.

---

## 👨‍💻 Proje Sahibi

| Bilgi | Değer |
| :--- | :--- |
| **Ad Soyad** | Samet ERDOĞAN |
| **Öğrenci Numarası** | 20230108039 |
| **Bölüm** | Bilgisayar Programcılığı |
| **Ders Adı** | Görsel Programlama |
| **Ders Kodu** | BIP2033 |
| **Öğretim Görevlisi** | Emrah SARIÇİÇEK |
| **Teslim Tarihi** | 05/12/2025 |

---

## 📌 Proje Hakkında

Bu **C#** uygulaması, `System.Net.Http` kütüphanesini kullanarak **HTTP GET** isteği yapar ve resmi tatil verilerini **JSON** formatında alır. Alınan veriler, `System.Text.Json` kullanılarak `Tatil` sınıfı nesnelerine dönüştürülür ve bellekte tutulur. Uygulama, kullanıcının seçimine göre bu veriler üzerinde filtreleme ve arama işlemleri gerçekleştirir.

### Kullanılan Veri Kaynağı:

* **API Adresi Şablonu:** `https://date.nager.at/api/v3/PublicHolidays/{YIL}/TR`
* **Desteklenen Yıllar:** `2023`, `2024`, `2025`

---

## 🚀 Özellikler

| İşlem | Açıklama |
| :--- | :--- |
| **Tatil Listeleme (Yıla Göre)** | Kullanıcının girdiği yıla (`2023`, `2024`, `2025`) ait tüm resmi tatilleri listeler. |
| **Tarihe Göre Arama** | Kullanıcının girdiği **gün-ay** formatına (`gg-aa`) uyan tatilleri, tarihin yıl kısmına bakmaksızın tüm yıllar için listeler. |
| **İsme Göre Arama** | Kullanıcının girdiği metni yerel veya uluslararası tatil adlarında büyük/küçük harf gözetmeksizin arar ve sonuçları listeler. |
| **Tümünü Gösterme** | Desteklenen tüm yıllara (`2023-2025`) ait tatilleri tarihe göre sıralayarak topluca listeler. |
| **Hata Kontrolleri** | Geçersiz yıl girişi veya arama metni için uygun uyarı mesajları gösterilir. |

---

## ⚙️ Gereksinimler

* **.NET SDK 8.0** veya üzeri (Proje C# ve modern .NET kütüphaneleri kullanır.)
* Tavsiye Edilen ve Geliştirilen IDE: **Visual Studio 2022 **.

---

## ▶️ Nasıl Çalıştırılır?

1.  Bu repoyu bilgisayarınıza **indirin** veya **klonlayın**.
2.  Projeyi bir **C# IDE'sinde** (örn: Visual Studio) açın.
3.  Proje ana dizinindeki **`Program.cs`** (veya projenizin ana dosyasını) bulun.
4.  `Main` metodu bulunan `TatilTakipci` sınıfını çalıştırın ve konsol ekranından menüdeki işlemleri seçerek uygulamayı yönetin.

### ✅ Örnek Kullanım

```shell
TÜRKİYE CUMHURİYETİ Resmi Tatil Takip Uygulaması Başlatılıyor...
Toplam 49 adet resmi tatil verisi yüklendi!

--- TÜRKİYE'deki Resmi Tatiller ---
1. Tatil listesini göster (yıl seçmeli)
2. Tarihe göre tatil ara (gg-aa formatı)
3. İsme göre tatil ara
4. Tüm tatilleri 3 yıl boyunca göster (2023–2024-2025)
5. Çıkış
------------------------------
Seçiminiz : 3

--- İsme Göre Ara ---
Aramak istediğiniz tatil adının bir kısmını girin (örn: zafer): zafer

** 'zafer' İçeren Tatiller **
Tarih: 2023-08-30 --> Yerel Ad: Zafer Bayramı                                 --> Uluslararası Ad: Victory Day
Tarih: 2024-08-30 --> Yerel Ad: Zafer Bayramı                                 --> Uluslararası Ad: Victory Day
Tarih: 2025-08-30 --> Yerel Ad: Zafer Bayramı                                 --> Uluslararası Ad: Victory Day
Toplam 3 tatil bulundu.

Devam etmek için bir tuşa basın...
