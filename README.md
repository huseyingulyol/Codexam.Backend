# CodExam Reader - Backend

Bu proje, öğretmenlerin yaptığı kodlama sınavlarını dijital ortamda yönetmek, el yazısı ile çözülmüş kod sınav kağıtlarını tanımak ve öğrencilere otomatik puanlama yapmak için geliştirilmiş bir backend uygulamasıdır.

Teknolojiler

- **ASP.NET Core 8.0 Web API**: Backend servisleri oluşturmak için.
- **Entity Framework Core**: ORM aracı, veritabanı işlemleri için.
- **SQLite**: Hafif, hızlı ve kolay veritabanı yönetimi.
- **Azure AI Vision (OCR)**: El yazısı kod sınav kağıtlarını dijital metne çevirmek için.
- **Google Gemini Generative AI**: Çözüm önerileri ve puanlama işlemleri için yapay zeka desteği.

Proje Yapısı

```
        ├── Controllers/              # API endpoint'leri
        ├── Entities/                 # Veritabanı modelleri/varlıkları
        ├── Migrations/      
        ├── Properties/    
        ├── Repositories/             # Varlıklara veritabanı düzeyinde işlem yaptığımız klasör
        └── uploads/                  # Öğretmenlerin yüklediği sınav sayfaları
        ├── Program.cs                # Uygulama başlangıç noktası
        ├── app.db                    # sqlite db dosyaları
        ├── app.db-shm
        ├── app.db-wal
        ├── appsettings.json          # Konfigürasyon dosyası
```

## Veri Tabanı Yapılandırması
![veri_tabani_yapilandirmasi](https://github.com/user-attachments/assets/65e53ba3-38c9-4363-ae29-4dbd6248e882)


Projenizin kök dizininde `appsettings.development.json` dosyasını bulundurmalısınız. Bu dosya aşağıdaki parametreleri içermelidir:

```json
{
  "Azure": {
    "SubscriptionKey": "GİZLİ_AZURE_ABONE_ANAHTARINIZ",
    "Endpoint": "https://codexam.cognitiveservices.azure.com/"
  },
  "Google": {
    "GeminiApiKey": "GİZLİ_GOOGLE_API_ANAHTARINIZ"
  },
  "TokenOptions": {
    "SecurityKey": "GİZLİ_GÜVENLİK_ANAHTARINIZ",
    "ExpirationTime": 10,
    "RefreshTokenTTL": 5,
    "Issuer": "com.codexam",
    "Audience": "com.codexam"
  }
}
```

## Kurulum

1. **Projeyi klonlayın:**
```bash
git clone https://github.com/kullanici-adi/codexam.backend.git
cd codexam.backend
```

2. **Gerekli bağımlılıkları yükleyin:**
```bash
dotnet restore
```

3. **Veritabanını oluşturun:**
```bash
dotnet ef database update
```

4. **Uygulamayı çalıştırın:**
```bash
dotnet run
```

## API Endpoint'leri

### Sınav Tanımlama
```
POST /api/Exam/Create
```
Öğretmen tarafından yeni bir kod sınavı tanımlanır.

### Çözümsüz Kod Sınav Kağıtlarını Yükleme
```
POST /api/TeacherPage/Upload
```
Öğretmen, sınavın saf halinin sayfalarını sisteme yükler.

### Çözümü Doğrulanmış Kod Sınav Kağıtlarını Yükleme
```
POST /api/TeacherPage/Upload
```
Öğretmen, kendi çözdüğü sınav sayfalarını sisteme yükler.

### Öğrencilerin Çözdüğü Kod Sınav Kağıtlarını Yükleme
```
POST /api/StudentPage/Upload
```
Öğretmen, öğrencinin el yazısıyla çözdüğü kod kağıdını sisteme yükler.


## Katkıda Bulunma
Katkıda bulunmak isterseniz lütfen bir issue açın veya pull request gönderin. Her türlü geri bildirim memnuniyetle karşılanır.

## Lisans
MIT Lisansı. Daha fazla bilgi için `LICENSE` dosyasına göz atabilirsiniz.

---

# CodExam Reader - Rapor

## 1. Giriş

Bu proje, React Native kullanarak Android Studio üzerinde çalışan bir mobil uygulama geliştirmeyi amaçlamaktadır. Uygulama, öğretmenlerin programlama sınav kağıtlarını OCR teknolojisi ile okuyarak değerlendirmesine yardımcı olacaktır. Projenin temel amacı, sınav değerlendirme sürecini hızlandırmak ve OCR teknolojisini optimize ederek daha doğru metin çıktıları elde etmektir.

## 2. Proje Kapsamı ve Hedefler

- Akademisyenden çözümsüz sınav kağıdı, cevap anahtarı ve öğrencilerin çözdüğü sınav kağıtlarını almak.
- Sınav kağıtlarının OCR teknolojisi ile dijital ortama aktarılması.
- Öğrenci kimlik bilgilerinin algılanarak kaydedilmesi.
- Öğrencinin programlama sınavlarında yaptığı sözdizimsel hataların akademisyenin inisiyatifine bağlı olarak tercihe bırakılması.
- Boşluk doldurma ve çoktan seçmeli soruların, öğretmenin yüklediği cevap anahtarı ile karşılaştırılarak otomatik puanlandırılması.
- Nerelerden ve neden puan kırıldığı bilgisine akademisyenin ulaşabilmesi.
- Küçük yazım hataları veya eş anlamlı kelimeler için tolerans mekanizması uygulanması.
- Kesin ve tek cevapları olan soruların doğru yanıtlarla karşılaştırılarak puanlanması.
- Açık uçlu ve kodlama sorularının Gemini AI ile doğruluk analizine tabi tutularak değerlendirilmesi.
- Kullanıcı dostu bir arayüz ile öğretmenlerin sınav değerlendirme sürecinin kolaylaştırılması.

### 2.1. Ulaşılan Hedefler

- Güçlü bir OCR ile metinler görsel içinde algılanıyor. Algılanan metin, katı kurallara sahip bir prompt ile dil modeline gönderilerek işleniyor ve istenen çıktı elde ediliyor.
- Veritabanı kurgusu başarıyla tamamlandı. Ancak kısıtlı zamanda çözülemeyen bir hatadan dolayı etkin şekilde çalışmıyor.

### 2.2. Ulaşılamayan Hedefler

Çözümlerin analizi ve puanlandırma işlemleri hedeflendi ancak gerçekleştirilemedi. Bu hedeflerin gerçekleşememesinin sebebi plansal ve tekniksel hatalar değil, zamanın kısıtlı olmasıdır.

## 3. Kullanılan Teknolojiler

### Frontend:
- **React Native:** Mobil uygulamanın geliştirilmesi için kullanılan framework.
- **TypeScript (TSX):** React Native bileşenlerinin daha güvenli kodlanması için kullanılmıştır.

### Backend:
- **C# ASP.NET Core 8 Web API**
- **EntityFramework ORM**
- **Azure OCR:** Görüntüleri metne çevirmek için Microsoft’un bulut tabanlı OCR hizmeti.
- **Gemini AI:** Açık uçlu soruların değerlendirilmesi ve doğruluk analizinin yapılması için kullanılan yapay zeka servisi.
- **SQLite:** Öğrenci, sınav ve değerlendirme verilerinin saklanması için kullanılan, hafif ve yerel bir veritabanı çözümü.

## 4. İşleyiş ve Geliştirme Süreci

### 4.1. İşleyiş

- Öğretmen, çözümsüz sınav kağıdını ve ardından çözülmüş sınav kağıdını sisteme yükler.
- Sınav kağıtları OCR teknolojisi ile dijital ortama aktarılır ve gerekli bilgileri çeker.
- Öğrenci sayfaları sırasıyla eklenir ve öğrenci kimlik bilgileri OCR ile algılanarak veritabanına kaydedilir.
- Kesin cevaplı sorular, öğretmenin belirttiği cevaplarla karşılaştırılarak puanlanır.
- Açık uçlu ve kodlama soruları, OCR ile metne çevrilip Gemini AI tarafından analiz edilerek puanlandırılır.

### 4.2. Geliştirme Süreci

- **Planlama:** Proje gereksinimleri belirlendi, hedefler oluşturuldu.
- **Tasarım:** Kullanıcı arayüzü ve sistem mimarisi tasarlandı.
- **Kodlama:** React Native ile frontend, ASP.NET Core ile backend geliştirildi. OCR ve AI entegrasyonları yapıldı.
- **Test ve Optimizasyon:** OCR doğruluğu test edildi, AI değerlendirme süreçleri optimize edildi ve performans iyileştirmeleri yapıldı.

### 4.3. Eklenecekler

- Öğrenci no veya ismi hatalı girildiğinde okulun veritabanında fuzzy matching yapılarak tahmin yapılması.
- En yüksek benzerlik oranına sahip eşleşmelerin öneri olarak sunulması.

## 5. Geliştirilebilirlik

- Öğrencilerin kendi yazdığı kodların doğruluğunu kontrol edebilmesi.
- Öğrencilerin sınav sonuçlarına uygulama aracılığıyla bakabilmesi.
- Gelişmiş OCR eğitimi, dil modeli ve yapay zeka teknolojilerinden yararlanılarak sorunsuz çalışan bir uygulama haline getirilebilir.

## 6. Sonuçlar ve Değerlendirme

Bu proje sayesinde sınav değerlendirme süreçleri daha hızlı ve hatasız hale getirilmesi amaçladık ama sonuçlandıramadık.
Azure OCR ve Gemini AI entegrasyonunu başarıyla sağladık ve yapay zeka desteğiyle etkili sonuçlar elde ettik.

### Kaynaklar

- [Handwritten Code Recognition for Pen-and-Paper CS Education](https://stanford.edu/~cpiech/bio/papers/handwrittencode.pdf)
- [React Native Introduction](https://reactnative.dev/docs/getting-started)
- [Quickstart: Azure AI Vision v3.2 GA Read](https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/quickstarts-sdk/client-library)
- [Gemini API reference](https://ai.google.dev/api?lang=python)
