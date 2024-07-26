using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordReturnDTO
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string ContactNo { get; set; }
        public string PatientType { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Medication { get; set; }
        public string TreatmentStatus { get; set; }

        public MedicalRecordReturnDTO(int recordId, int patientId, string patientName, string contactNo, string patientType, DateTime date, string diagnosis, string treatment, string medication, string treatmentStatus)
        {
            RecordId = recordId;
            PatientId = patientId;
            PatientName = patientName;
            ContactNo = contactNo;
            PatientType = patientType;
            Date = date;
            Diagnosis = diagnosis;
            Treatment = treatment;
            Medication = medication;
            TreatmentStatus = treatmentStatus;
        }
    }
}
