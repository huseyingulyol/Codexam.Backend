using Codexam.Domain.Enums;

namespace Codexam.WebAPI.DTOs
{
    public class PaperPageUploadDto
    {
        public int ExamId { get; set; }
        public IFormFile File { get; set; }
        public PageType PageType { get; set; }  

    }
}
