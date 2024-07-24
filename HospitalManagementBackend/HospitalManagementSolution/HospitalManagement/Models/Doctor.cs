using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Doctor 
    {
        [Key]
        public int DoctorId { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public string Qualification { get; set; }
        public string LanguagesKnown { get; set; }
        public TimeOnly ShiftStartTime { get; set; }
        public TimeOnly ShiftEndTime { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }

    }
}
