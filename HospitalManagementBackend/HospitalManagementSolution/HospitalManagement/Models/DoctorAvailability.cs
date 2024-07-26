using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class DoctorAvailability
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public List<TimeOnly> AvailableSlots { get; set; }

        public DoctorAvailability(int doctorId, List<TimeOnly> availableSlots)
        {
            DoctorId = doctorId;
            AvailableSlots = availableSlots;
        }
    }
}
