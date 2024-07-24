namespace HospitalManagement.Models.DTOs
{
    public class DoctorReturnDTO
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public string LanguagesKnown { get; set; }
        public TimeOnly ShiftStartTime { get; set; }
        public TimeOnly ShiftEndTime { get; set; }

        public DoctorReturnDTO(int doctorId,string doctorName, string specialization, int experience, string languagesKnown, TimeOnly shiftStartTime, TimeOnly shiftEndTime)
        {
            DoctorId = doctorId;
            DoctorName = doctorName;
            Specialization = specialization;
            Experience = experience;
            LanguagesKnown = languagesKnown;
            ShiftStartTime = shiftStartTime;
            ShiftEndTime = shiftEndTime;
        }
    }
}
