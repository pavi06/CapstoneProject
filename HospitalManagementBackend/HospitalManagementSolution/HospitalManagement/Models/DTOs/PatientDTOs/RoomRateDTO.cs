namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class RoomRateDTO
    {
        public int NoOfDays { get; set; }
        public double CostPerDay { get; set; }

        public RoomRateDTO(int noOfDays, double costPerDay)
        {
            NoOfDays = noOfDays;
            CostPerDay = costPerDay;
        }
    }
}
