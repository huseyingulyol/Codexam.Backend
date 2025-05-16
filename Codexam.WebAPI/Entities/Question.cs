namespace Codexam.WebAPI.Entities
{
    public class Question : BaseEntity
    {
        public int TeacherPageId { get; set; }
        public string Type { get; set; }
        public byte Number { get; set; }
        public string Content { get;  set; }
  
        public int ParentId { get; set; }
        public byte Score { get; set; }

        public virtual Question Parent { get; set; }
        public virtual TeacherPage TeacherPage { get; set; }

    }
}
