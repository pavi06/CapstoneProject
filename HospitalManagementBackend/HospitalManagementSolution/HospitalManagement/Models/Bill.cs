using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public string PatientType { get; set; }
        public string Description { get; set; }
        public Double Amount { get; set; }
        public string PaymentStatus { get; set; } = "Not Paid";

        public Bill(int patientId, string patientType, string description, double amount)
        {
            PatientId = patientId;
            PatientType = patientType;
            Description = description;
            Amount = amount;
        }
    }
}
