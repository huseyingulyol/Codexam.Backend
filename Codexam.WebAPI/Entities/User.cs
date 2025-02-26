using System.Text.Json.Serialization;

namespace Codexam.WebAPI.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public virtual Role? Role { get; set; }
    }

}
