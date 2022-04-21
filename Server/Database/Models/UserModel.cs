using System.ComponentModel.DataAnnotations;

namespace Server.Database.Models
{
    public class UserModel
    {
        [Key]
        public string User { get; set; } = null!;
        public string KeyHash { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
    }
}