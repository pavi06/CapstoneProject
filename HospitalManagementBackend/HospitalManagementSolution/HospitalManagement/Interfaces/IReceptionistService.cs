using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.BillDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IReceptionistService
    {
        public Task<List<DoctorAvailabilityDTO>> CheckDoctoravailability(string specialization, int limit, int skip);
        public Task<int> BookAppointment(ReceptionistBookAppointmentDTO appointmentDTO);
        public Task<Dictionary<string, int>> CheckBedAvailability();
        public Task<string> CreateInPatient(InPatientDTO patientDTO);
        public Task<string> UpdateInPatient(UpdateInPatientDTO patientDTO);
        public Task<OutPatientBillDTO> GenerateBillForOutPatient(int appointmentid);
        public Task<InPatientBillDTO> GenerateBillForInPatient(int inPatientid);
        public Task<string> UpdateInPatientDetailsForDischarge(int inpatientId, int billId);
        public Task<ReceptAppointmentReturnDTO> GetAppointmentDetails(int appointmentid);
        public Task<List<ReceptAppointmentReturnDTO>> GetAllTodayAppointments(int limit, int skip);
        public Task<List<PendingBillReturnDTO>> GetPendingBills();
        public Task<List<InPatientReturnDTO>> GetAllInPatientDetails();
        public Task<string> AddDoctorForInPatient(AddDoctorDTO dto);

    }
}
