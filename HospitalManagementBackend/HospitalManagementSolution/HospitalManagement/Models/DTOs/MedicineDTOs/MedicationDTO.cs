using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.MedicineDTOs
{
    public class MedicationDTO
    {
        [Required(ErrorMessage = "Medicine name cannot be empty")]
        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Medicine form cannot be empty")]
        public string Form { get; set; }

        [Required(ErrorMessage = "Dosage cannot be empty")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Provide intake timing, it cannot be empty")]
        public string IntakeTiming { get; set; }

        [Required(ErrorMessage = "Provide intake, it cannot be empty")]
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
