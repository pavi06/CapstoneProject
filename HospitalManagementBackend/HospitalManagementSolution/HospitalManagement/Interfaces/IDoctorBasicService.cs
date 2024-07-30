using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IDoctorBasicService
    {
        public Task<List<DoctorReturnDTO>> MapDoctors(List<Doctor> doctors);
        public Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization);
        public Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization, int limit, int skip);
        public Task<List<string>> GetAllSpecializations();
        public Task<int> GetPatientId(PatientFindDTO patientDTO);
    }
}
