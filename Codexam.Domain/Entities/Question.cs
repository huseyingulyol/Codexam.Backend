namespace Codexam.Domain.Entities
{
    public class Question : BaseEntity
    {
        public int ExamId { get; set; }
        public string Type { get; set; }
        public byte Number { get; set; }
        public string Content { get;  set; }
        public int ParentId { get; set; }
        public byte Score { get; set; }

        public virtual Question Parent { get; set; }

    }
}
