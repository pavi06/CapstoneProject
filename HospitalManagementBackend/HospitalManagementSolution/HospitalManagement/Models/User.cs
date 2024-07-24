using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class User
    {
        [Key]
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public UserDetails Person { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordHashKey { get; set; }
        public string Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        public User() { }
    }
}
