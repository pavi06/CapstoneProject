namespace HospitalManagement.Models.DTOs.UserDTOs
{
    public class UserRegistrationDTO
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

    }
}
