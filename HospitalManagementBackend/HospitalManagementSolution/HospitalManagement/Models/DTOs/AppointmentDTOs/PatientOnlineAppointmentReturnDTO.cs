namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class PatientOnlineAppointmentReturnDTO : PatientAppointmentReturnDTO
    {
        public string MeetLink { get; set; }

        public PatientOnlineAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, string slotConfirmed, string description, string doctorName, string specialization, string appointmentStatus, string appointmentType, string meetLink)
            : base(appointmentId, appointmentDate, slotConfirmed, description, doctorName, specialization, appointmentStatus, appointmentType)
        {
            MeetLink = meetLink;
        }
    }
}
