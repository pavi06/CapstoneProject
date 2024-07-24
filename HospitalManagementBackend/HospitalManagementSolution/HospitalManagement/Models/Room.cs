using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public int WardTypeId { get; set; }
        [ForeignKey("WardTypeId")]
        public WardBedAvailability WardBed { get; set; }
    }
}
