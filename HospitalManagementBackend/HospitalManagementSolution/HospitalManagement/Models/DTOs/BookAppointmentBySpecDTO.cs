namespace HospitalManagement.Models.DTOs
{
    public class BookAppointmentBySpecDTO
    {
        public int PatientId { get; set; }
        public string PhoneNo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PreferredTime { get; set; }
        public string Speciality { get; set; }
        public string Description { get; set; }
        public string PreferredLanguage { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentMode { get; set; }

        public BookAppointmentBySpecDTO(int patientId, string phoneNo, DateTime appointmentDate,string preferredTime, string speciality, string description, string preferredLanguage, string appointmentType, string appointmentMode)
        {
            PatientId = patientId;
            PhoneNo = phoneNo;
            AppointmentDate = appointmentDate;
            PreferredTime = preferredTime;
            Speciality = speciality;
            Description = description;
            PreferredLanguage = preferredLanguage;
            AppointmentType = appointmentType;
            AppointmentMode = appointmentMode;
        }
    }
}
