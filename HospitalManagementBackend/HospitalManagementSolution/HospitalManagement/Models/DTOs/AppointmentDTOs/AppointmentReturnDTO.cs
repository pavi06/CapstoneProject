namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class AppointmentReturnDTO
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly Slot { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string ContactNo { get; set; }
        public string Description { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentStatus { get; set; }

        public AppointmentReturnDTO(int appointmentId, DateTime appointmentDate, TimeOnly slot, int patientId, string patientName, int age, string contactNo, string description, string appointmentType, string appointmentStatus)
        {
            AppointmentId = appointmentId;
            AppointmentDate = appointmentDate;
            Slot = slot;
            PatientId = patientId;
            PatientName = patientName;
            Age = age;
            ContactNo = contactNo;
            Description = description;
            AppointmentType = appointmentType;
            AppointmentStatus = appointmentStatus;
        }
    }
}
