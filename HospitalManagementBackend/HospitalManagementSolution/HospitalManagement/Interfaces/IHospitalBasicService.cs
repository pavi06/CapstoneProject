using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.DoctorDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IHospitalBasicService
    {
        public Task<List<DoctorReturnDTO>> MapDoctors(List<Doctor> doctors);
        public Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization);
        public Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization, int limit, int skip);
    }
}
