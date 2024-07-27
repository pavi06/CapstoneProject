using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class WardRoomsAvailability
    {
        [Key]
        public int WardTypeId { get; set; }
        public string WardType { get; set; }
        public int RoomsAvailable { get; set; }
        public int TotalRoomsCount { get; set; }
        public List<Room> Rooms { get; set; }

    }
}
