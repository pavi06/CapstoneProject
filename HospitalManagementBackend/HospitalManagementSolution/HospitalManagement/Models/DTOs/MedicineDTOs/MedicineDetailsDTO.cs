namespace HospitalManagement.Models.DTOs.MedicineDTOs
{
    public class MedicineDetailsDTO
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public List<string> DosagesAvailable { get; set; }
        public List<string> FormsAvailable { get; set; }

        public MedicineDetailsDTO(int medicineId, string medicineName, List<string> dosagesAvailable, List<string> formsAvailable)
        {
            MedicineId = medicineId;
            MedicineName = medicineName;
            DosagesAvailable = dosagesAvailable;
            FormsAvailable = formsAvailable;
        }
    }
}
