using HospitalManagement.Models;
using HospitalManagement.Models.DTOs;

namespace HospitalManagement.Interfaces
{
    public interface IPatientService
    {
        public Task<PatientAppointmentReturnDTO> BookAppointmentByDoctor(BookAppointmentDTO appointmentDTO);
        public Task<PatientAppointmentReturnDTO> BookAppointmentBySpeciality(BookAppointmentBySpecDTO specAppointmentDTO);
        public Task<List<PatientAppointmentReturnDTO>> MyAppointments(int patientId, int limit, int skip);
        public Task<Prescription> MyPrescriptionForAppointment(int patientId, int appointmentId);
        public Task<List<Prescription>> MyPrescriptions(int patientId, int limit, int skip);
        public Task<string> CancelAppointment(int appointmentId, string status);
    }
}
