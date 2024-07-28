using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IPatientService
    {
        public Task<PatientAppointmentReturnDTO> BookAppointmentByDoctor(BookAppointmentDTO appointmentDTO);
        public Task<Dictionary<string, bool>> GetAvailableSlotsOfDoctor(CheckDoctorSlotsDTO checkSlotsDTO);
        public Task<PatientAppointmentReturnDTO> BookAppointmentBySpeciality(BookAppointmentBySpecDTO specAppointmentDTO);
        public Task<List<PatientAppointmentReturnDTO>> MyAppointments(int patientId, int limit, int skip);
        public Task<PrescriptionReturnDTO> MyPrescriptionForAppointment(int patientId, int appointmentId);
        public Task<List<PrescriptionReturnDTO>> MyPrescriptions(int patientId, int limit, int skip);
        public Task<string> CancelAppointment(int appointmentId);
        public Task<Doctor> GetDoctorAvailableOnThatSlot(string speciality, string preferredTime, string preferredLanguage,DateTime appointmentDate);
    }
}
