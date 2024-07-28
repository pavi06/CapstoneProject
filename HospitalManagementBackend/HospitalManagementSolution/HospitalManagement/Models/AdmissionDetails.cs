using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class AdmissionDetails
    {
        [Key]
        public int AdmissionDetailsId { get; set; }
        public int AdmissionId { get; set; }
        public Admission Admission { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public int NoOfDays { get; set; }

        public AdmissionDetails(int admissionId, int roomId, int noOfDays)
        {
            AdmissionId = admissionId;
            RoomId = roomId;
            NoOfDays = noOfDays;
        }
    }
}
