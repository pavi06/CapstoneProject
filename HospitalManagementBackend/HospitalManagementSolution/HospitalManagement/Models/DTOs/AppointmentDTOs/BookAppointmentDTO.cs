using HospitalManagement.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.DTOs.AppointmentDTOs
{
    public class BookAppointmentDTO
    {
        [Required(ErrorMessage = "PatientId cannot be null")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient phone number cannot be empty")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Choose appointmentDate, it cannot be empty")]
        [DataType(DataType.Date , ErrorMessage ="Appointmentdate Should be of datetime format")]
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
        [Required(ErrorMessage = "Choose appointment mode, it cannot be null")]
        public string AppointmentMode { get; set; }

        public BookAppointmentDTO(int patientId, string contactNo, DateTime appointmentDate, string slot, int doctorId, string description, string appointmentType, string appointmentMode)
        {
            PatientId = patientId;
            ContactNo = contactNo;
            AppointmentDate = appointmentDate;
            Slot = slot;
            DoctorId = doctorId;
            Description = description;
            AppointmentType = appointmentType;
            AppointmentMode = appointmentMode;
        }

    }
}
