using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class MedicineMaster
    {
        [Key]
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public List<string> DosagesAvailable { get; set; }
        public List<string> FormsAvailable { get; set; }
    }
}
