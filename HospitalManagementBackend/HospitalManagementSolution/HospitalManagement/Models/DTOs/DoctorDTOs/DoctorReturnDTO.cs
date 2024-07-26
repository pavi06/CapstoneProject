namespace HospitalManagement.Models.DTOs.DoctorDTOs
{
    public class DoctorReturnDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public List<string> LanguagesKnown { get; set; }
        public List<string> AvailableDaysOfWeek { get; set; }
        public Dictionary<TimeOnly, bool> AvailableSlots { get; set; }

        public DoctorReturnDTO(int doctorId, string doctorName, string specialization, int experience, List<string> languagesKnown, List<string> availableDaysOfWeek, Dictionary<TimeOnly, bool> availableSlots)
        {
            DoctorId = doctorId;
            DoctorName = doctorName;
            Specialization = specialization;
            Experience = experience;
            LanguagesKnown = languagesKnown;
            AvailableDaysOfWeek = availableDaysOfWeek;
            AvailableSlots = availableSlots;
        }

    }
}
