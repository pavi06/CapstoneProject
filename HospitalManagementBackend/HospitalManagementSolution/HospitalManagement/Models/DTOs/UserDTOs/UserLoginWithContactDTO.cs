using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.UserDTOs
{
    public class UserLoginWithContactDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName cannot be empty")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ContactNumber cannot be empty")]
        [StringLength(15)]
        public string ContactNumber { get; set; }
    }
}
