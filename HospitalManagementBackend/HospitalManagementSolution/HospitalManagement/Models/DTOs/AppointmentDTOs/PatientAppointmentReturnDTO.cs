namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class PatientAppointmentReturnDTO
    {
        public int AppointmentId { get; set; }
        //date?? when req raised
        public DateTime AppointmentDate { get; set; }
        public string SlotConfirmed { get; set; }
        public string Description { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string AppointmentStatus { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentMode { get; set; }
        public string MeetLink { get; set; }

        public PatientAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, string slotConfirmed, string description, string doctorName, string specialization, string appointmentStatus, string mode , string appointmentType,string meetLink)
        {
            AppointmentId = appointmentId;
            AppointmentDate = appointmentDate;
            SlotConfirmed = slotConfirmed;
            Description = description;
            DoctorName = doctorName;
            Specialization = specialization;
            AppointmentStatus = appointmentStatus;
            AppointmentMode = mode;
            AppointmentType = appointmentType;            
            MeetLink = meetLink;
        }

        public PatientAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, string slotConfirmed, string description, string doctorName, string specialization, string appointmentStatus,string mode ,string appointmentType)
        {
            AppointmentId = appointmentId;
            AppointmentDate = appointmentDate;
            SlotConfirmed = slotConfirmed;
            Description = description;
            DoctorName = doctorName;
            Specialization = specialization;
            AppointmentStatus = appointmentStatus;
            AppointmentMode = mode;
            AppointmentType = appointmentType;
        }
    }
}
