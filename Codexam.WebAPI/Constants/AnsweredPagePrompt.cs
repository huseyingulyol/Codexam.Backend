namespace Codexam.WebAPI.Constants
{
    public class AnsweredPagePrompt : IPrompt
    {
        public string GetPrompt() =>
            """
            Aşağıda bir sınav kağıdının OCR çıktısı verilecektir. Bu çıktıyı dikkatlice incele ve aşağıdaki kurallara göre yapılandırılmış bir JSON çıktısı üret:

            Kurallar:

            1. **Kimlik Bilgileri:**
               - Öğrencinin adı ve soyadını metin içinde arayıp bul. Eğer bulursan "name" anahtarına yaz.
               - Eğer T.C. Kimlik Numarası bulunuyorsa "identity_number" anahtarına yaz.
               - Öğrenci numarası (okul numarası) varsa "student_number" anahtarına yaz.

            2. **Soru Cevapları:**
               - Öğrenci tarafından verilen tüm cevapları analiz et.
               - Her bir cevabı ayrı bir obje olarak yaz. Her cevabın:
                 - "no" (soru numarası)
                 - "content" (verilen cevabın metni)
                 alanları olsun.

            3. **Soru Numaralandırma Tespiti:**
               Öğrenci soruları aşağıdaki yöntemlerden biriyle numaralandırmış olabilir:
               - Sorunun hemen altına cevabını yazar (bu durumda cevabın konumuna göre sorunun sırasını tahmin et).
               - Cevabın başında “1)”, “2.” gibi açık bir numaralandırma varsa, bu numarayı "no" olarak al.
               - “1.sorunun devamı”, “2. soru devamı” gibi ifadeler varsa bu cevabı önceki sorunun devamı olarak değerlendir.
               - Numara belirtmeyen veya belirsiz durumlar varsa cevap sırasına göre "no" belirle (örneğin ilk bulduğun numarasız cevaba 1 ver).

            Çıktıyı aşağıdaki örneğe uygun olarak düzenle:

            ```json
            {
              "name": "Öğrencinin Adı Soyadı (varsa)",
              "student_number": "Öğrenci Numarası (varsa)",
              "identity_number": "T.C. Kimlik Numarası (varsa)",
              "answers": [
                {
                  "no": 1,
                  "content": "1. soruya verilen cevap metni buraya gelecek..."
                },
                {
                  "no": 2,
                  "content": "2. soruya verilen cevap metni buraya gelecek..."
                }
                // Diğer cevaplar aynı formatta devam edecek
              ]
            }
            Sadece yukarıdaki formatta JSON üret. Açıklama veya başka içerik yazma. Boş değerler varsa ilgili alanı hiç ekleme.

            OCR Çıktısı:
            """;

    }
}
