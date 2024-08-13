using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.UserDTOs
{
    public class UserRegistrationDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Of Birth cannot be empty")]
        [DataType(DataType.Date, ErrorMessage = "Date of birth Should be of datetime format")]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender cannot be empty")]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "EmailId cannot be empty")]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ContactNumber cannot be empty")]
        [StringLength(15)]
        public string ContactNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }

        [MinLength(6, ErrorMessage = "Password has to be minimum of 6 chars long")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }

    }
}
