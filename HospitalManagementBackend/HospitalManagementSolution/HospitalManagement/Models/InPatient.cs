using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class InPatient
    {
        [Key]
        public int InPatientId { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor DoctorInCharge { get; set; }
        public DateTime AdmittedDate { get; set; } = DateTime.Now.Date;
        public string Description { get; set; }
        public DateTime? DischargeDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int? BillId { get; set; }
        public Bill GeneratedBill { get; set; }
        public List<InPatientDetails> InPatientDetails { get; set; }
        public List<Bill> Bills { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }

        public InPatient(int inPatientId, int doctorId, string description)
        {
            InPatientId = inPatientId;
            DoctorId = doctorId;
            Description = description;
        }
    }
}
