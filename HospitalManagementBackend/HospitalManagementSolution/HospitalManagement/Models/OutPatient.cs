using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class OutPatient
    {
        [Key]
        public int OutPatientId { get; set; }
        public List<Bill> Bills { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
