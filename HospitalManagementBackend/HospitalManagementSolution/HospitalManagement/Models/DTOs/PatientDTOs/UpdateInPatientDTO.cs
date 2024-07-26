namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class UpdateInPatientDTO
    {
        public int PatientId { get; set; }
        public string WardType { get; set; }
        public int NoOfDays { get; set; }

        public UpdateInPatientDTO(int patientId, string wardType, int noOfDays)
        {
            PatientId = patientId;
            WardType = wardType;
            NoOfDays = noOfDays;
        }
    }
}
