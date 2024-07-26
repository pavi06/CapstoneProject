namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class InPatientDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string WardType { get; set; }
        public int NoOfDays { get; set; }
        public string Description { get; set; }

        public InPatientDTO(int patientId, int doctorId, string wardType, int noOfDays, string description)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            WardType = wardType;
            NoOfDays = noOfDays;
            Description = description;
        }
    }
}
