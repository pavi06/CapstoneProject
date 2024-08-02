using HospitalManagement.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.DoctorDTOs
{
    public class CheckDoctorSlotsDTO
    {
        [Required(ErrorMessage = "Choose the doctor, it cannot be empty")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Choose Date, it cannot be empty")]
        [DataType(DataType.Date, ErrorMessage = "date Should be of datetime format")]
        [CustomDateValidation(ErrorMessage = "Date should be greater than or equal to today")]
        public DateTime Date { get; set; }
    }
}
