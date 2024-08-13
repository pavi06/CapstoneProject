using HospitalManagement.Models.DTOs.MedicineDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class ProvidePrescriptionDTO
    {
        [Required(ErrorMessage = "Doctorid cannot be empty")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Patient id cannot be empty")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient type cannot be empty")]
        public string PatientType { get; set; }

        [Required(ErrorMessage = "Prescription for cannot be empty")]
        public int PrescriptionFor { get; set; }

        [Required(ErrorMessage = "prescription medicine list cannot be empty")]
        public List<MedicationDTO> prescribedMedicine { get; set; }
    }
}
