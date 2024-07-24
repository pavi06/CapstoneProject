using HospitalManagement.Models.DTOs;

namespace HospitalManagement.Interfaces
{
    public interface IHospitalBasicService
    {
        public Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization);
        public Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization, int limit, int skip);
    }
}
