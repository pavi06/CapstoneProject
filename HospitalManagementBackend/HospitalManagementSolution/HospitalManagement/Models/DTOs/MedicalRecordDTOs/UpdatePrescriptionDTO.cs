using HospitalManagement.Models.DTOs.MedicineDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class UpdatePrescriptionDTO
    {
        [Required(ErrorMessage = "PrescriptionId cannot be empty")]
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "Prescription medicineList cannot be empty")]
        public List<MedicationDTO> prescribedMedicine { get; set; }
    }
}
