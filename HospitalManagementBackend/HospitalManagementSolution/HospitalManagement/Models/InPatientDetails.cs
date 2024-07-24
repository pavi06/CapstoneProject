using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class InPatientDetails
    {
        [Key]
        public int InPatientDetailsId { get; set; }
        public int InPatientId { get; set; }
        public InPatient InPatient { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public int NoOfDays { get; set; }

    }
}
