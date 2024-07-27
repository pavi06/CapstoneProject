using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordReturnDTO
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string ContactNo { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public List<Medication> Medications { get; set; }
        public string TreatmentStatus { get; set; }

        public MedicalRecordReturnDTO(int recordId, int patientId, string patientName, string contactNo, DateTime date, string diagnosis, string treatment, List<Medication> medication, string treatmentStatus)
        {
            RecordId = recordId;
            PatientId = patientId;
            PatientName = patientName;
            ContactNo = contactNo;
            Date = date;
            Diagnosis = diagnosis;
            Treatment = treatment;
            Medications = medication;
            TreatmentStatus = treatmentStatus;
        }
    }
}
