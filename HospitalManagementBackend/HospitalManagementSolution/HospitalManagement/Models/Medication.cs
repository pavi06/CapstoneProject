using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }
        public string MedicineName { get; set; }
        public string Form { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
        public string IntakeTiming { get; set; }
        public string Intake { get; set; }
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        public Medication(string medicineName, string form, string dosage, int quantity, string intakeTiming, string intake, int prescriptionId)
        {
            MedicineName = medicineName;
            Form = form;
            Dosage = dosage;
            Quantity = quantity;
            IntakeTiming = intakeTiming;
            Intake = intake;
            PrescriptionId = prescriptionId;
        }
    }
}
