using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class UpdateInPatientDTO
    {
        [Required(ErrorMessage = "Admission id cannot be empty")]
        public int AdmissionId { get; set; }

        [Required(ErrorMessage = "Wardtype cannot be empty")]
        public string WardType { get; set; }

        [Required(ErrorMessage = "No of days cannot be empty")]
        [Range(1, 100, ErrorMessage = "Value must be >=1")]
        public int NoOfDays { get; set; }

        public UpdateInPatientDTO(int admissionId, string wardType, int noOfDays)
        {
            AdmissionId = admissionId;
            WardType = wardType;
            NoOfDays = noOfDays;
        }
    }
}
