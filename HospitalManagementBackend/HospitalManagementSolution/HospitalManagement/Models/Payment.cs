using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
