using HospitalManagement.Models.DTOs.MedicineDTOs;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class AppointmentMedicalRecordDTO
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientType { get; set; }
        public int DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string TreatmentStatus { get; set; }

        public AppointmentMedicalRecordDTO(int patientId, string patientType, int doctorId, string diagnosis, string treatment, string treatmentStatus)
        {
            PatientId = patientId;
            PatientType = patientType;
            DoctorId = doctorId;
            Diagnosis = diagnosis;
            Treatment = treatment;
            TreatmentStatus = treatmentStatus;
        }
    }
}
