//using Codexam.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Codexam.WebAPI.Entities;

namespace Codexam.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TeacherPageController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        private const string UPLOAD_DIR = "uploads";

        public List<VisualFeatureTypes?> features =
            new List<VisualFeatureTypes?>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        public TeacherPageController(IConfiguration configuration,HttpClient httpClient, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _httpClient = httpClient;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] int examId)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded" });

            try
            {
                Directory.CreateDirectory(UPLOAD_DIR);
                var filePath = Path.Combine(UPLOAD_DIR, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var maxNumber = await _context.TeacherPages.MaxAsync(tp => (int?)tp.Number) ?? 0;

                await CreatePage(new TeacherPage()
                {
                    ExamId = examId,
                    Number = maxNumber + 1,
                    Url = filePath,
                    isSolved = false,
                });


                ComputerVisionClient computerVision = new ComputerVisionClient(
             new ApiKeyServiceClientCredentials(_configuration["Azure:SubscriptionKey"]),
             new System.Net.Http.DelegatingHandler[] { });

                computerVision.Endpoint = _configuration["Azure:Endpoint"];

                string extractedText = await ReadTextFromImage(computerVision, filePath);
                string result = await PostCorrection(extractedText);

                //string jsonHam = result.Replace("```json","").Replace("```","").Replace("\n","").Replace("\")



                return Ok(new
                {
                    message = "Successfully",
                    filename = file.FileName,
                    response = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }


   
        private async Task<string> ReadTextFromImage(ComputerVisionClient client, string imagePath)
        {
            using var imageStream = System.IO.File.OpenRead(imagePath);
            var readResponse = await client.ReadInStreamAsync(imageStream);
            string operationLocation = readResponse.OperationLocation;
            string operationId = operationLocation.Split('/').Last();

            ReadOperationResult result;

            do
            {
                result = await client.GetReadResultAsync(Guid.Parse(operationId));
                await Task.Delay(1000);
            }
            while (result.Status == OperationStatusCodes.Running || result.Status == OperationStatusCodes.NotStarted);

            if (result.Status != OperationStatusCodes.Succeeded)
            {
                return "Failed to extract text.";
            }

            var extractedText = new System.Text.StringBuilder();

            foreach (var page in result.AnalyzeResult.ReadResults)
            {
                foreach (var line in page.Lines)
                {
                    extractedText.AppendLine(line.Text);
                }
            }

            return extractedText.ToString();
        }
        private async Task<string> PostCorrection(string ocrResult)
        {
            string _prompt =
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

            if (string.IsNullOrWhiteSpace(ocrResult))
                return ("OCR result cannot be empty");

            try
            {
                var requestBody = new
                {
                    model = "gemini-2.0-flash-exp",
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = _prompt + ocrResult }
                            }
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");//GeminiApiKey
                var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={_configuration["Google:GeminiApiKey"]}", content);

                if (!response.IsSuccessStatusCode)
                    return $"Error from Gemini API: {await response.Content.ReadAsStringAsync()}";

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                //var text = result.GetProperty("text").GetString();
                var text = result.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();


                if (string.IsNullOrWhiteSpace(text))
                    return "Failed to get a valid response from Gemini API";

                return text;
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        private async Task<ActionResult<IEnumerable<TeacherPage>>> GetPages()
        {
            return await _context.TeacherPages.ToListAsync();
        }
        private async Task<ActionResult<TeacherPage>> CreatePage(TeacherPage product)
        {
            _context.TeacherPages.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPages), new { id = product.Number }, product);
        }
    }
}
