namespace HospitalManagement.Models.DTOs.DoctorDTOs
{
    public class DoctorAvailabilityDTO
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public List<string> KnownLanguages { get; set; }
        public List<string> AvailableDays { get; set; }
        public DateTime Date { get; set; }
        public List<TimeOnly> AvailableSlots { get; set; }

        public DoctorAvailabilityDTO(int doctorId, string name, string specialization, int experience, List<string> knownLanguages, List<string> availableDays, DateTime date, List<TimeOnly> availableSlots)
        {
            DoctorId = doctorId;
            Name = name;
            Specialization = specialization;
            Experience = experience;
            KnownLanguages = knownLanguages;
            AvailableDays = availableDays;
            Date = date;
            AvailableSlots = availableSlots;
        }

    }
}
