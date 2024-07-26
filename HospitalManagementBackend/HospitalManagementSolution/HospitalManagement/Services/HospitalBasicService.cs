using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Repositories;

namespace HospitalManagement.Services
{
    public class HospitalBasicService : IHospitalBasicService
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, UserDetails> _userDetailsRepository;
        private readonly IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> _doctorAvailabilityRepository;

        public HospitalBasicService(IRepository<int, Doctor> doctorRepository, IRepository<int, UserDetails> userDetailsRepository, IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> doctorAvailabilityRepository) { 
            _doctorRepository = doctorRepository;
            _userDetailsRepository = userDetailsRepository;
            _doctorAvailabilityRepository = doctorAvailabilityRepository;
        }

        #region MapDoctors
        public async Task<List<DoctorReturnDTO>> MapDoctors(List<Doctor> doctors)
        {
            var doctorList = await Task.WhenAll(doctors.Select(async d =>
            {
                var doctor = await _userDetailsRepository.Get(d.DoctorId);
                var doctorAvailability = await _doctorAvailabilityRepository.Get(d.DoctorId, DateTime.Now.Date);
                Dictionary<TimeOnly, bool> slotsAvailable = new Dictionary<TimeOnly, bool>();
                if (doctorAvailability == null)
                {                    
                    foreach (var slot in d.Slots)
                    {
                      slotsAvailable.Add(slot, true);
                    }
                }
                else
                {
                    foreach (var slot in d.Slots)
                    {
                        if (doctorAvailability.AvailableSlots.Contains(slot))
                        {
                            slotsAvailable.Add(slot, true);
                        }
                        else
                        {
                            slotsAvailable.Add(slot, false);
                        }
                    }
                }
                return new DoctorReturnDTO(d.DoctorId, doctor.Name, d.Specialization, d.Experience, d.LanguagesKnown, d.AvailableDays, slotsAvailable);
            }));
            return doctorList.ToList();
        }
        #endregion

        #region DoctorsBySpecialization
        public async Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization)
        {
            var doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == specialization);
            if (doctors.ToList().Count == 0)
            {
                throw new ObjectsNotAvailableException("Doctors");
            }
            try
            {
                return await MapDoctors(doctors.ToList());
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
           
        }
        #endregion

        #region DoctorsBySpecializationWithLimit
        public async Task<List<DoctorReturnDTO>> GetAllDoctorsBySpecialization(string specialization, int limit, int skip)
        {
            var doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == specialization).Skip(skip).Take(limit);
            if (doctors.ToList().Count == 0)
            {
                throw new ObjectsNotAvailableException("Doctors");
            }
            try
            {
                return await MapDoctors(doctors.ToList());
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion
    }
}
