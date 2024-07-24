using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public string PatientType { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Medication { get; set; }
        public string TreatmentStatus { get; set; }
    }
}
