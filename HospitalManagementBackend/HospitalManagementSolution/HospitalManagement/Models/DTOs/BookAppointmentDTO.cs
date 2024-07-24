namespace HospitalManagement.Models.DTOs
{
    public class BookAppointmentDTO
    {
        public int PatientId { get; set; }
        public string ContactNo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly Slot { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentMode { get; set; }

        public BookAppointmentDTO(int patientId, string contactNo, DateTime appointmentDate, TimeOnly slot, int doctorId, string description, string appointmentType, string appointmentMode)
        {
            PatientId = patientId;
            ContactNo = contactNo;
            AppointmentDate = appointmentDate;
            Slot = slot;
            DoctorId = doctorId;
            Description = description;
            AppointmentType = appointmentType;
            AppointmentMode = appointmentMode;
        }

    }
}
