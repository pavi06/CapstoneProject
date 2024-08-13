using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class DoctorAvailability
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public List<TimeOnly> AvailableSlots { get; set; }

        public DoctorAvailability(int doctorId, DateTime appointmentDate,List<TimeOnly> availableSlots)
        {
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            AvailableSlots = availableSlots;
        }
    }
}
