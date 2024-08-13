using HospitalManagement.Models.DTOs.MedicineDTOs;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class AppointmentMedicalRecordDTO
    {
        [Required(ErrorMessage = "Appointment id cannot be empty")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Patient id cannot be empty")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Provide patient type, it cannot be empty")]
        public string PatientType { get; set; }

        [Required(ErrorMessage = "Choose the doctor, it cannot be empty")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Diagnosis field cannot be empty")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Treatment field cannot be empty")]
        public string Treatment { get; set; }

        [Required(ErrorMessage = "Choose the treatment status, it cannot be empty")]
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
