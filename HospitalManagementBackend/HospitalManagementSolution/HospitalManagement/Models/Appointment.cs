using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public DateTime AppointmentDate { get; set; }
        public TimeOnly Slot { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string Speciality { get; set; }
        public int PatientId { get; set; }
        public OutPatient Patient { get; set; }
        public string Description { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentMode { get; set; }

        public Appointment() { }

        public Appointment(DateTime appointmentDate, TimeOnly slot, int doctorId, string speciality, int patientId, string description, string appointmentStatus, string appointmentType, string appointmentMode)
        {
            AppointmentDate = appointmentDate;
            Slot = slot;
            DoctorId = doctorId;
            Speciality = speciality;
            PatientId = patientId;
            Description = description;
            AppointmentStatus = appointmentStatus;
            AppointmentType = appointmentType;
            AppointmentMode = appointmentMode;
        }
    }
}
