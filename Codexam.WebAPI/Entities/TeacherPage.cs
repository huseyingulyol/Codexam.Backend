namespace Codexam.WebAPI.Entities
{
    public class TeacherPage : BaseEntity
    {
        public int ExamId { get; set; }
        public int Number { get; set; }
        public string Url { get; set; }
        public bool isSolved { get; set; }

        public virtual Exam Exam { get; set; }

    }
}
