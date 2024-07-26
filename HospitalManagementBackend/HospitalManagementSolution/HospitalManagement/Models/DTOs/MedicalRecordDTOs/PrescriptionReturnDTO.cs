using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class PrescriptionReturnDTO
    {
        public int PrescriptionId { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string DocSpecialization { get; set; }
        public string PrescriptionUrl { get; set; }

        public PrescriptionReturnDTO(int prescriptionId, int appointmentId, string patientName, string doctorName, string docSpecialization, string prescriptionUrl)
        {
            PrescriptionId = prescriptionId;
            AppointmentId = appointmentId;
            PatientName = patientName;
            DoctorName = doctorName;
            DocSpecialization = docSpecialization;
            PrescriptionUrl = prescriptionUrl;
        }
    }
}
