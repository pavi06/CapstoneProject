namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class DoctorAppointmentReturnDTO : AppointmentReturnDTO
    {
        public bool PrescriptionAddedOrNot { get; set; }
        public string  MeetLink { get; set; }

        public DoctorAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, TimeOnly slot, 
            int patientId, string patientName, int age, string contactNo, string description, string appointmentType, string mode, string meetLink,
            string appointmentStatus, bool prescriptionStatus):base(appointmentId, appointmentDate,slot, patientId, patientName, age, contactNo, description, appointmentType,mode,appointmentStatus)
        {
            PrescriptionAddedOrNot = prescriptionStatus;
            MeetLink = meetLink;
        }

        public DoctorAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, TimeOnly slot,
           int patientId, string patientName, int age, string contactNo, string description, string appointmentType, string mode,
           string appointmentStatus, bool prescriptionStatus) : base(appointmentId, appointmentDate, slot, patientId, patientName, age, contactNo, description, appointmentType, mode, appointmentStatus)
        {
            PrescriptionAddedOrNot = prescriptionStatus;
        }
    }
}
