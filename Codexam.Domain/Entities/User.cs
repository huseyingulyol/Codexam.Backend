using System.Text.Json.Serialization;

namespace Codexam.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public byte[]? PasswordHash { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Role? Role { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }

}
