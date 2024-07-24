using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class UserDetails
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; } 
        public string Female { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
