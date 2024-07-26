namespace HospitalManagement.Models.DTOs.PatientDTOs
{
    public class OutPatientBillDTO
    {
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string ContactNo { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }
        public double DoctorFee { get; set; }
        public double TotalAmount { get; set; }

        public OutPatientBillDTO(int billId, int patientId, string patientName, int age, string contactNo, string doctorName, string doctorSpecialization, double doctorFee, double totalAmount)
        {
            BillId = billId;
            PatientId = patientId;
            PatientName = patientName;
            Age = age;
            ContactNo = contactNo;
            DoctorName = doctorName;
            DoctorSpecialization = doctorSpecialization;
            DoctorFee = doctorFee;
            TotalAmount = totalAmount;
        }
    }
}
