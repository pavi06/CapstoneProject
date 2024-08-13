using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PrescriptionFor { get; set; }
        public List<Medication> Medications { get; set; }

    }
}
