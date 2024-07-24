using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs;
using HospitalManagement.Repositories;

namespace HospitalManagement.Services
{
    public class HospitalBasicService : IHospitalBasicService
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, UserDetails> _userDetailsRepository;

        public HospitalBasicService(IRepository<int, Doctor> doctorRepository, IRepository<int, UserDetails> userDetailsRepository) { 
            _doctorRepository = doctorRepository;
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization)
        {
            var doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == specialization);
            var doctorList = await Task.WhenAll(doctors.Select(async d =>
            {
                var doctor = await _userDetailsRepository.Get(d.DoctorId);
                return new DoctorReturnDTO(d.DoctorId,doctor.Name, d.Specialization, d.Experience, d.LanguagesKnown, d.ShiftStartTime, d.ShiftEndTime);
            }));
            return doctorList.ToList();
        }

        public async Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization, int limit, int skip)
        {
            var doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == specialization).Skip(skip).Take(limit);
            var doctorList = await Task.WhenAll(doctors.Select(async d =>
            {
                var doctor = await _userDetailsRepository.Get(d.DoctorId);
                return new DoctorReturnDTO(d.DoctorId, doctor.Name, d.Specialization, d.Experience, d.LanguagesKnown, d.ShiftStartTime, d.ShiftEndTime);
            }));
            return doctorList.ToList();
        }
    }
}
