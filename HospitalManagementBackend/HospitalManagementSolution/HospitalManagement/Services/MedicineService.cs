using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using HospitalManagement.Models.DTOs.MedicineDTOs;
using HospitalManagement.Repositories;

namespace HospitalManagement.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IRepository<int, MedicineMaster> _medicineMasterRepository;

        public MedicineService(IRepository<int, MedicineMaster> medicineRepository) {
            _medicineMasterRepository = medicineRepository;
        }

        public async Task<List<MedicineReturnDTO>> GetAllMedicineNames()
        {
            var medicines = await _medicineMasterRepository.Get();
            if(medicines == null)
            {
                throw new ObjectsNotAvailableException("Medicines");
            }
            var medicineList = await Task.WhenAll(medicines.Select(async m =>
            {
                return new MedicineReturnDTO(m.MedicineId, m.MedicineName);
            }));
            return medicineList.ToList();
        }

        public async Task<MedicineDetailsDTO> GetMedicineDetailsById(int medicineId)
        {
            try
            {
                var medicine = await _medicineMasterRepository.Get(medicineId);
                return new MedicineDetailsDTO(medicine.MedicineId, medicine.MedicineName, medicine.DosagesAvailable, medicine.FormsAvailable);
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
    }
}
