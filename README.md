![veri_tabani_yapilandirmasi](https://github.com/user-attachments/assets/e9e74505-ffd0-4b0d-b25b-694e793bfab7)CodExam Reader - Backend

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
        ├── DTos/                     # Backend frontend veri aktarırken varlığa kullanılan veri şeması
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

### Çözümsüz Kod Sınavı Tanımlama
```
POST /api/exams
```
Öğretmen tarafından yeni bir kod sınavı tanımlanır.

### Çözülmüş Kod Sınav Kağıdı Yükleme
```
POST /api/exams/{examId}/papers
```
Öğretmen, öğrencinin el yazısıyla çözdüğü kod kağıdını sisteme yükler.

### OCR İşlemi
```
POST /api/exams/{examId}/papers/{paperId}/ocr
```
Azure AI Vision ile kağıt üzerindeki el yazısını dijital metne çevirir.

### Otomatik Puanlama
```
POST /api/exams/{examId}/papers/{paperId}/evaluate
```
Google Gemini AI kullanarak kod kağıdını değerlendirir ve puan verir.

## Çevre Değişkenleri

Uygulamanın çalışabilmesi için aşağıdaki environment variable'ların tanımlanması gerekmektedir:

```
AZURE_OCR_API_KEY=<Azure OCR API anahtarınız>
GOOGLE_GEMINI_API_KEY=<Google Gemini API anahtarınız>
```

## Katkıda Bulunma

Katkıda bulunmak isterseniz lütfen bir issue açın veya pull request gönderin. Her türlü geri bildirim memnuniyetle karşılanır.

## Lisans

MIT Lisansı. Daha fazla bilgi için `LICENSE` dosyasına göz atabilirsiniz.



Katkıda Bulunma

Katkıda bulunmak isterseniz lütfen bir issue açın veya pull request gönderin. Her türlü geri bildirim memnuniyetle karşılanır.

Lisans

MIT Lisansı. Daha fazla bilgi için LICENSE dosyasına göz atabilirsiniz.
