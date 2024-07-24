using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class InPatientRepository:AbstractRepository<int, InPatient>
    {
        public InPatientRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<InPatient> Delete(int key)
        {
            try
            {
                var inPatient = await Get(key);
                _context.Remove(inPatient);
                await _context.SaveChangesAsync();
                return inPatient;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("InPatient");
            }

        }

        public override async Task<InPatient> Get(int key)
        {
            var inPatient = await _context.InPatients.Include(p => p.Bills).SingleOrDefaultAsync(u => u.InPatientId == key);
            if (inPatient != null)
            {
                return inPatient;
            }
            throw new ObjectNotAvailableException("InPatient");
        }

        public override async Task<IEnumerable<InPatient>> Get()
        {
            var inPatients = await _context.InPatients.Include(p => p.Bills).ToListAsync();
            return inPatients;
        }

        public override async Task<InPatient> Update(InPatient item)
        {
            try
            {
                if (await Get(item.InPatientId) != null)
                {
                    _context.Entry<InPatient>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("InPatient");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("InPatient");
            }

        }
    }
}
