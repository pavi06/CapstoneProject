namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class InPatientReturnDTO
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string WardType { get; set; }
        public int RoomNo { get; set; }
        public DateTime AdmittedDate { get; set; }
        public string Description { get; set; }

        public InPatientReturnDTO(int patientId, string patientName, string wardType, int roomNo, DateTime admittedDate, string description)
        {
            PatientId = patientId;
            PatientName = patientName;
            WardType = wardType;
            RoomNo = roomNo;
            AdmittedDate = admittedDate;
            Description = description;
        }
    }
}
