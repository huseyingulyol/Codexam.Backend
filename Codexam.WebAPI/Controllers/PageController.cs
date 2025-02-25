using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codexam.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private const string UPLOAD_DIR = "uploads";


        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
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

                var ocrResult = await RunOcrAsync(filePath);
                var response = await PostCorrectionAsync(ocrResult);

                return Ok(new
                {
                    message = "Successfully",
                    filename = file.FileName,
                    response = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }


        // Dummy OCR function (implement as needed)
        private Task<string> RunOcrAsync(string filePath)
        {
            // Implement your OCR logic here
            return Task.FromResult("OCR result");
        }

        // Dummy post-correction function (implement as needed)
        private Task<string> PostCorrectionAsync(string ocrResult)
        {
            // Implement your post-correction logic here
            return Task.FromResult("Post-correction response");
        }
    }
}
