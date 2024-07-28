namespace HospitalManagement.Models.DTOs.MedicineDTOs
{
    public class MedicationMapperDTO : MedicationDTO
    {
        public int MedicationId { get; set; }
        public int PrescriptionId { get; set; }

        public MedicationMapperDTO(string medicineName, string form, string dosage, int quantity, string intakeTiming, string intake,int medicationId, int prescriptionId):
            base(medicineName,form,dosage,quantity,intakeTiming,intake)
        {
            MedicationId = medicationId;
            PrescriptionId = prescriptionId;
        }
    }
}
