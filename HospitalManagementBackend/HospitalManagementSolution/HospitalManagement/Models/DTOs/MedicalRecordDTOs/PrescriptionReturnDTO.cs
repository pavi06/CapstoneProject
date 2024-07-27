using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class PrescriptionReturnDTO
    {
        public int PrescriptionId { get; set; }
        public int PrescriptionFor { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string DoctorName { get; set; }
        public string DocSpecialization { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public List<Medication> Prescription { get; set; }

        public PrescriptionReturnDTO(int prescriptionId, int prescriptionFor, int patientId, string patientName, int age, string doctorName, string docSpecialization, List<Medication> prescription)
        {
            PrescriptionId = prescriptionId;
            PrescriptionFor = prescriptionFor;
            PatientId = patientId;
            PatientName = patientName;
            Age = age;
            DoctorName = doctorName;
            DocSpecialization = docSpecialization;
            Prescription = prescription;
        }
    }
}
