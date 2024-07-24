using HospitalManagement.Models;
using HospitalManagement.Models.DTOs;

namespace HospitalManagement.Interfaces
{
    public interface IDoctorService
    {
        public Task<List<AppointmentReturnDTO>> GetAllTodayAppointments(int doctorid);
        public Task<Prescription> UploadPrescriptionForAppointment(int appointmentid, string prescriptionImage);
        public Task<List<MedicalRecordReturnDTO>> GetPatientMedicalRecords(int patientid, string patientType, int doctorid);

    }
}
