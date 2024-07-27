using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IDoctorService
    {
        public Task<List<AppointmentReturnDTO>> GetAllTodayAppointments(int doctorid);
        public Task<string> CancelAppointment(int appointmentId);
        public Task<PrescriptionReturnDTO> ProvidePrescriptionForAppointment(ProvidePrescriptionDTO prescriptionDTO);
        public Task<List<MedicalRecordReturnDTO>> GetPatientMedicalRecords(int patientid, int doctorid);

    }
}
