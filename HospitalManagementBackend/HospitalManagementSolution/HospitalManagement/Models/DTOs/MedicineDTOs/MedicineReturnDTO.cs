namespace HospitalManagement.Models.DTOs.MedicineDTOs
{
    public class MedicineReturnDTO
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }

        public MedicineReturnDTO(int medicineId, string medicineName)
        {
            MedicineId = medicineId;
            MedicineName = medicineName;
        }
    }
}
