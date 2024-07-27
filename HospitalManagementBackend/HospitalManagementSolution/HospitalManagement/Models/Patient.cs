using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public User User { get; set; }
        public List<Bill> Bills { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Admission> Admissions { get; set; }
        public List<Prescription> Prescriptions { get; set; }

        public Patient(int patientId)
        {
            PatientId = patientId;
        }
    }
}
