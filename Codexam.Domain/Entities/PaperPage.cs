using Codexam.Domain.Enums;

namespace Codexam.Domain.Entities
{
    public class PaperPage : BaseEntity
    {
        public int ExamId { get; set; }
        public int Number { get; set; }
        public string FilePath { get; set; }
        public PageType Type { get; set; }
        public virtual Exam Exam { get; set; }


    }
}
