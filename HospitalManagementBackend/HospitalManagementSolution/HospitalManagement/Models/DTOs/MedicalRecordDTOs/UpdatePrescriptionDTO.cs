using HospitalManagement.Models.DTOs.MedicineDTOs;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class UpdatePrescriptionDTO
    {
        public int PrescriptionId { get; set; }
        public List<MedicationDTO> prescribedMedicine { get; set; }
    }
}
