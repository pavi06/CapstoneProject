namespace HospitalManagement.Models.DTOs.MedicineDTOs
{
    public class MedicationDTO
    {
        public string MedicineName { get; set; }
        public string Form { get; set; }
        public string Dosage { get; set; }
        public string IntakeTiming { get; set; }
        public string Intake { get; set; }

        public MedicationDTO(string medicineName, string form, string dosage, string intakeTiming, string intake)
        {
            MedicineName = medicineName;
            Form = form;
            Dosage = dosage;
            IntakeTiming = intakeTiming;
            Intake = intake;
        }
    }
}
