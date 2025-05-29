namespace Codexam.Domain.Entities
{
    public class Exam:BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Paper> TeacherPages { get; set; }
    }
}
