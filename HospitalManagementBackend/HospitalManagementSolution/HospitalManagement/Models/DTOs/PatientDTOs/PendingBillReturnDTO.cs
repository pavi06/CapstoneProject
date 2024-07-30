namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class PendingBillReturnDTO
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string ContactNo { get; set; }
        public int BillId { get; set; }
        public DateTime BillIssueDate { get; set; }
        public double TotalAmount { get; set; }
        public double BalanceAmount { get; set; }
        public string PaymentStatus { get; set; }

        public PendingBillReturnDTO(int patientId, string patientName, string contactNo, int billId, DateTime billIssueDate, double totalAmount, double balanceAmount, string paymentStatus)
        {
            PatientId = patientId;
            PatientName = patientName;
            ContactNo = contactNo;
            BillId = billId;
            BillIssueDate = billIssueDate;
            TotalAmount = totalAmount;
            BalanceAmount = balanceAmount;
            PaymentStatus = paymentStatus;
        }
    }
}
