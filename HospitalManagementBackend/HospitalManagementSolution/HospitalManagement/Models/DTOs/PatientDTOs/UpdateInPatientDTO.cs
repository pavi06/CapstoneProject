namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class UpdateInPatientDTO
    {
        public int AdmissionId { get; set; }
        public string WardType { get; set; }
        public int NoOfDays { get; set; }

        public UpdateInPatientDTO(int admissionId, string wardType, int noOfDays)
        {
            AdmissionId = admissionId;
            WardType = wardType;
            NoOfDays = noOfDays;
        }
    }
}
