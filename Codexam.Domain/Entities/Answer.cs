namespace Codexam.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }

        public virtual Question Question { get; set; }
    }
}
