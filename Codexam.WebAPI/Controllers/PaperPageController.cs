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
using Codexam.Domain.Entities;
using Codexam.WebAPI.Constants;
using Codexam.WebAPI.Persistence;
using Codexam.WebAPI.DTOs;
using Codexam.Domain.Models;
using Codexam.Domain.Enums;

namespace Codexam.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PaperPageController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly CodexamDbContext _context;
        private readonly IConfiguration _configuration;

        private const string UPLOAD_DIR = "uploads";

        public List<VisualFeatureTypes?> features =
            new List<VisualFeatureTypes?>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        public PaperPageController(IConfiguration configuration,HttpClient httpClient, CodexamDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _httpClient = httpClient;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] PaperPageUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest(new { error = "No file uploaded" });

            try
            {
                Directory.CreateDirectory(UPLOAD_DIR);
                var filePath = Path.Combine(UPLOAD_DIR, dto.File.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                var maxNumber = await _context.PaperPages.MaxAsync(tp => (int?)tp.Number) ?? 0;

                await CreatePage(new PaperPage()
                {
                    ExamId = dto.ExamId,
                    Number = maxNumber + 1,
                    FilePath = filePath,
                    Type = dto.PageType,
                });
                ComputerVisionClient computerVision = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(_configuration["Azure:SubscriptionKey"]));

                computerVision.Endpoint = _configuration["Azure:Endpoint"];

                string extractedText = await ReadTextFromImage(computerVision, filePath);

                string result = await PostCorrection(extractedText, dto.PageType);

                if (result.StartsWith("```json"))
                {
                    result = result.Substring(7).TrimStart(); // 7 karakter: ```json
                }

                if (result.EndsWith("```"))
                {
                    result = result.Substring(0, result.Length - 3).TrimEnd(); // sondaki ```
                }

                if (dto.PageType == PageType.Answered)
                {
                    PaperPageJson? page = JsonSerializer.Deserialize<PaperPageJson>(result);
                    if (page is not null)
                    {
                        foreach (var answer in page.answers)
                        {
                            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Number == answer.no && q.ExamId == dto.ExamId);
                            if (question != null)
                            {
                                question.Content = answer.content;
                                _context.Questions.Update(question);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                }

                return Ok(new
                {
                    message = "Successfully",
                    filename = dto.File.FileName,
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
        private async Task<string> PostCorrection(string ocrResult, PageType pageType)
        {
            
            IPrompt prompt = pageType switch
            {
                PageType.Unsolved => new UnsolvedPagePrompt(),
                PageType.Answered => new AnsweredPagePrompt(),
                _ => throw new ArgumentOutOfRangeException(nameof(pageType), pageType, null)
            };

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

                                new { text = prompt.GetPrompt() + ocrResult }
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

        private async Task<ActionResult<IEnumerable<PaperPage>>> GetPages()
        {
            return await _context.PaperPages.ToListAsync();
        }
        private async Task<ActionResult<PaperPage>> CreatePage(PaperPage product)
        {
            _context.PaperPages.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPages), new { id = product.Number }, product);
        }
    }
}
