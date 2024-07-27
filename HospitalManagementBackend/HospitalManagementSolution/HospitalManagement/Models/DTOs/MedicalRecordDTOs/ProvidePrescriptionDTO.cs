using HospitalManagement.Models.DTOs.MedicineDTOs;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class ProvidePrescriptionDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string PatientType { get; set; }
        public int PrescriptionFor { get; set; }
        public List<MedicationDTO> prescribedMedicine { get; set; }
    }
}
