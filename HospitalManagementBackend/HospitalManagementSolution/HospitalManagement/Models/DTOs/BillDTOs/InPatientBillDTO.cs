namespace HospitalManagement.Models.DTOs.BillDTOs
{
    public class InPatientBillDTO
    {
        public int BillId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string ContactNo { get; set; }
        public DateTime AdmittedDate { get; set; }
        public int NoOfDaysIn { get; set; }
        public Dictionary<string, RoomRateDTO> RoomDetails { get; set; }
        public double DoctorFee { get; set; }
        public double TotalAmount { get; set; }

        public InPatientBillDTO(int billId, DateTime date, int patientId, string patientName, int age, string contactNo, DateTime admittedDate, int noOfDaysIn, Dictionary<string, RoomRateDTO> roomDetails, double doctorFee, double totalAmount)
        {
            BillId = billId;
            Date = date;
            PatientId = patientId;
            PatientName = patientName;
            Age = age;
            ContactNo = contactNo;
            AdmittedDate = admittedDate;
            NoOfDaysIn = noOfDaysIn;
            RoomDetails = roomDetails;
            DoctorFee = doctorFee;
            TotalAmount = totalAmount;
        }
    }
}
