using HospitalManagement.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class BookAppointmentBySpecDTO
    {
        [Required(ErrorMessage = "PatientId cannot be null")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Patient phone number cannot be empty")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Choose appointmentDate, it cannot be empty")]
        [DataType(DataType.Date, ErrorMessage = "Appointmentdate Should be of datetime format")]
        [CustomDateValidation(ErrorMessage = "Date should be greater than or equal to today")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Preferred time cannot be empty")]
        public string PreferredTime { get; set; }

        [Required(ErrorMessage = "Speciality cannot be empty")]
        public string Speciality { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Preferred language cannot be empty")]
        public string PreferredLanguage { get; set; }

        [Required(ErrorMessage = "Choose appointment type, it cannot be null")]
        public string AppointmentType { get; set; }

        [Required(ErrorMessage = "Choose appointment mode, it cannot be null")]
        public string AppointmentMode { get; set; }

        public BookAppointmentBySpecDTO(int patientId, string phoneNo, DateTime appointmentDate, string preferredTime, string speciality, string description, string preferredLanguage, string appointmentType, string appointmentMode)
        {
            PatientId = patientId;
            PhoneNo = phoneNo;
            AppointmentDate = appointmentDate;
            PreferredTime = preferredTime;
            Speciality = speciality;
            Description = description;
            PreferredLanguage = preferredLanguage;
            AppointmentType = appointmentType;
            AppointmentMode = appointmentMode;
        }
    }
}
