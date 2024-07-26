using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Doctor 
    {
        [Key]
        public int DoctorId { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public string Qualification { get; set; }
        public List<string> LanguagesKnown { get; set; }
        public TimeOnly ShiftStartTime { get; set; }
        public TimeOnly ShiftEndTime { get; set; }
        public List<TimeOnly> Slots { get; set; }
        public List<string> AvailableDays { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }

    }
}
