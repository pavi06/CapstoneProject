using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IReceptionistService
    {
        public Task<List<DoctorAvailabilityDTO>> CheckDoctoravailability(string specialization);
        public Task<List<Doctor>> GetAllDoctorsAvailableNow(string specialization, TimeOnly timeOfDay, DateTime date);
        public Task<ReceptAppointmentReturnDTO> BookAppointment(BookAppointmentDTO appointmentDTO);
        public Task<Dictionary<string, int>> CheckBedAvailability();
        public Task<string> CreateInPatient(InPatientDTO patientDTO);
        public Task<string> UpdateInPatient(UpdateInPatientDTO patientDTO);
        public Task<string> CancelAppointment(int appointmentId);
        public Task<OutPatientBillDTO> GenerateBillForOutPatient(int appointmentid);
        public Task<InPatientBillDTO> GenerateBillForInPatient(int inPatientid);
        public Task<string> UpdateInPatientDetailsForDischarge(int inpatientId, int billId);
        public Task<ReceptAppointmentReturnDTO> GetAppointmentDetails(int appointmentid);

    }
}
