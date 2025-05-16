namespace Codexam.WebAPI.Entities
{
    public class Exam:BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TeacherPage> TeacherPages { get; set; }
    }
}
