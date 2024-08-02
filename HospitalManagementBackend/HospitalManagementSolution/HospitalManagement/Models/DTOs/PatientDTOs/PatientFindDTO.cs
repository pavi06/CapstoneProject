using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class PatientFindDTO
    {
        [Required(ErrorMessage = "Patient name cannot be empty")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "ContactNumber cannot be empty")]
        [StringLength(15)]
        public string ContactNumber { get; set; }
    }
}
