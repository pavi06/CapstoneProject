using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class DoctorAvailability
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public List<string> AvailableSlots { get; set; }
    }
}
