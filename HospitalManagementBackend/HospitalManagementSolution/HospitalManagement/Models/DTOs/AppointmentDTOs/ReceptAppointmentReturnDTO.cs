namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class ReceptAppointmentReturnDTO : AppointmentReturnDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }

        public ReceptAppointmentReturnDTO(int appointmentId, DateTime appointmentDate, TimeOnly slot, int patientId, string patientName,
            int age, string contactNo, string description, string appointmentType,string mode, string appointmentStatus, int doctorId, string doctorName, string specialization)
            : base(appointmentId, appointmentDate, slot, patientId, patientName, age, contactNo, description, appointmentType,mode, appointmentStatus)
        {
            DoctorId = doctorId;
            DoctorName = doctorName;
            Specialization = specialization;
        }
    }
}
