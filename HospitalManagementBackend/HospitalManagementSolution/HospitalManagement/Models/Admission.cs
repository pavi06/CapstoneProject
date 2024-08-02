using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Admission
    {
        [Key]
        public int AdmissionId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int? DoctorId { get; set; }
        public Doctor DoctorInCharge { get; set; }
        public DateTime AdmittedDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public DateTime? DischargeDate { get; set; }
        public bool IsActivePatient { get; set; } = true;
        public List<AdmissionDetails> AdmissionDetails { get; set; }

        public Admission(int patientId, int doctorId, string description)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            Description = description;
        }

        public Admission(int patientId, string description)
        {
            PatientId = patientId;
            Description = description;
        }
    }
}
