namespace HospitalManagement.Models.DTOs.MedicalRecordDTOs
{
    public class PrescriptionsReturnDTO
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescribedDate { get; set; }
        public int PrescriptionFor { get; set; }

        public PrescriptionsReturnDTO(int prescriptionId, DateTime prescribedDate, int prescriptionFor)
        {
            PrescriptionId = prescriptionId;
            PrescribedDate = prescribedDate;
            PrescriptionFor = prescriptionFor;
        }
    }
}
