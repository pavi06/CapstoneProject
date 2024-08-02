namespace HospitalManagement.Models.DTOs.BillDTOs
{
    public class RoomRateDTO
    {
        public string RoomType { get; set; }
        public int NoOfDays { get; set; }
        public double CostPerDay { get; set; }

        public RoomRateDTO(string roomType, int noOfDays, double costPerDay)
        {
            RoomType = roomType;
            NoOfDays = noOfDays;
            CostPerDay = costPerDay;
        }
    }
}
