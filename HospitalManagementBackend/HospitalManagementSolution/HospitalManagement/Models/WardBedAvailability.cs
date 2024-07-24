using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class WardBedAvailability
    {
        [Key]
        public int WardBedTypeId { get; set; }
        public string WardBedType { get; set; }
        public int BedsAvailable { get; set; }
        public int TotalBedCount { get; set; }
        public Double CostsPerDay { get; set; }
        public List<Room> Rooms { get; set; }

    }
}
