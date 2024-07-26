namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class PatientAppointmentReturnDTO
    {
        public int AppointmentId { get; set; }
        //date?? when req raised
        public DateTime AppointmentDate { get; set; }
        public TimeOnly SlotConfirmed { get; set; }
        public string Description { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentType { get; set; }

        public PatientAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, TimeOnly slotConfirmed, string description, string doctorName, string specialization, string appointmentStatus, string appointmentType)
        {
            AppointmentId = appointmentId;
            AppointmentDate = appointmentDate;
            SlotConfirmed = slotConfirmed;
            Description = description;
            DoctorName = doctorName;
            Specialization = specialization;
            AppointmentStatus = appointmentStatus;
            AppointmentType = appointmentType;
        }
    }
}
