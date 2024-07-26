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
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } = "User";

        public UserDetails(string name, DateTime dateOfBirth, int age, string gender, string emailId, string contactNo, string address)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Age = age;
            Gender = gender;
            EmailId = emailId;
            ContactNo = contactNo;
            Address = address;
        }
    }
}
