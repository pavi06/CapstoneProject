namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class ReceptionistBookAppointmentDTO
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Slot { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; }
        public string AppointmentType { get; set; }

    }
}
