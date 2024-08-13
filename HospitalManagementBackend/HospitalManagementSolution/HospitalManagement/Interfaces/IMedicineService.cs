using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.MedicineDTOs;

namespace HospitalManagement.Interfaces
{
    public interface IMedicineService
    { 
        public Task<List<MedicineReturnDTO>> GetAllMedicineNames();
        public Task<MedicineDetailsDTO> GetMedicineDetailsById(int medicineId);
    }
}
