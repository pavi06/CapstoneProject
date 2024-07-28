namespace HospitalManagement.Models.DTOs.MedicineDTOs
{
    public class MedicationDTO
    {
        public string MedicineName { get; set; }
        public string Form { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
        public string IntakeTiming { get; set; }
        public string Intake { get; set; }

        public MedicationDTO(string medicineName, string form, string dosage, int quantity, string intakeTiming, string intake)
        {
            MedicineName = medicineName;
            Form = form;
            Dosage = dosage;
            Quantity = quantity;
            IntakeTiming = intakeTiming;
            Intake = intake;
        }
    }
}
