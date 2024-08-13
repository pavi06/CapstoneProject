using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public int BillGeneratedFor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string PatientType { get; set; }
        public string Description { get; set; }
        public Double Amount { get; set; }
        public string PaymentStatus { get; set; } = "Not Paid";
        public List<Payment> Payments { get; set; }

        public Bill() { }

        public Bill(int billGeneratedFor, int patientId, string patientType, string description, double amount)
        {
            BillGeneratedFor = billGeneratedFor;
            PatientId = patientId;
            PatientType = patientType;
            Description = description;
            Amount = amount;
        }
    }
}
