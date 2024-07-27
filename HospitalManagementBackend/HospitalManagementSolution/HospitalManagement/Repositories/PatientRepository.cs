using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class PatientRepository : AbstractRepository<int, Patient>
    {
        public PatientRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Patient> Delete(int key)
        {
            try
            {
                var patient = await Get(key);
                _context.Remove(patient);
                await _context.SaveChangesAsync();
                return patient;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Patient");
            }

        }

        public override async Task<Patient> Get(int key)
        {
            var patient = await _context.Patients.Include(p=>p.Appointments).Include(p => p.Bills).SingleOrDefaultAsync(u => u.PatientId == key);
            return patient;
        }

        public override async Task<IEnumerable<Patient>> Get()
        {
            var patients = await _context.Patients.Include(p => p.Appointments).Include(p => p.Bills).ToListAsync();
            return patients;
        }

        public override async Task<Patient> Update(Patient item)
        {
            try
            {
                if (await Get(item.PatientId) != null)
                {
                    _context.Entry<Patient>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("Patient");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Patient");
            }

        }
    }
}
