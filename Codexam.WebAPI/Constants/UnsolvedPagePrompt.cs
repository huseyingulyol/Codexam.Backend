namespace Codexam.WebAPI.Constants
{
    public class UnsolvedPagePrompt : IPrompt
    {
        public string GetPrompt() => 
            """
            Amaç: Kodlama sınavını çözen bir uygulama yaptık. Şuanda senden istediğimiz aşağıdaki verilen OCR çıktısını bir düzenlemen.


            Kurallar:
            Varolandan hariç hiç bir ekstra kelime ekleme veya silme yapma.
            Algılayacağın soru tipleri bunlardır: "ACIK_UCLU", "KOD_YAZMA", "CIKTI_TAHMIN","BOSLUK_DOLDURMA", "DOGRU_YALNIS"
                başka hiç bir tip dahil etme eğer burdakilerden farklı soru tipi varsa dahil etme!
            Eğer sorunun yanında puan yazıyor ise "score" olarak ver. Eğer soruya ait bir puanlandırma yoksa "Belirsiz" diye belirt.
            Eğer soru bir önceki başlığa aitse yani soru bir alt başlık sorusuysa bunu farkedip o sorunun içinde konumlandır.
            Aşağıdaki bir örnek:
            {
              .
              .
              question:[
                .
                .
                questionNo:a
                question:"soru"
                .
                .
              ]
              .
              .
            }

            Önemli:
            Varolan yazım hatalarını mantıksal ve sözdizimsel olarak analiz ederek düzenle.
            JSON tipinde sana verilen formatla birebir şekilde bir çıktı ver.

            Format:
            questions:[
              {
                questionType:"CIKTI_TAHMIN",
                questionNo:1,
                question: [
                  questionType:"",
                  questionNo:1,
                  question:
                  questionScore:10,
                ]
                questionScore:10,

              },
              {
                questionType:"",
                questionNo:2,
                question:"Aşağıdaki lisp prog.a... 1 cümleyle açıkla.\n(defun))"
                questionScore:10,

              },
            ]

            OCR Çıktısı:
            """;

    }
}
