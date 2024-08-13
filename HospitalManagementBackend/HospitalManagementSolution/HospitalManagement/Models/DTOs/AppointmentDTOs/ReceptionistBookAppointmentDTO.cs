using HospitalManagement.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class ReceptionistBookAppointmentDTO
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Choose appointmentDate, it cannot be empty")]
        [DataType(DataType.Date, ErrorMessage = "Appointmentdate Should be of datetime format")]
        [CustomDateValidation(ErrorMessage = "Date should be greater than or equal to today")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Choose the preferred slot, it cannot be empty")]
        public string Slot { get; set; }

        [Required(ErrorMessage = "Choose the doctor, it cannot be empty")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Provide description, it cannot be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Choose appointment type, it cannot be null")]
        public string AppointmentType { get; set; }

    }
}
