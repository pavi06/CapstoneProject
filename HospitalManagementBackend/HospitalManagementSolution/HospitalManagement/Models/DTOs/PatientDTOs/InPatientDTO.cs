namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class InPatientDTO
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string WardType { get; set; }
        public int NoOfDays { get; set; }
        public string Description { get; set; }

        public InPatientDTO(string name, DateTime dateOfBirth, string gender, string contactNo, string address, string wardType, int noOfDays, string description)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            ContactNo = contactNo;
            Address = address;
            WardType = wardType;
            NoOfDays = noOfDays;
            Description = description;
        }

        public InPatientDTO(int patientId, string wardType, int noOfDays, string description)
        {
            PatientId = patientId;
            WardType = wardType;
            NoOfDays = noOfDays;
            Description = description;
        }
    }
}
