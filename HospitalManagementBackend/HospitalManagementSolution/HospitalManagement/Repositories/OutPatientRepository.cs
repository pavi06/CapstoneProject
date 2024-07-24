using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class OutPatientRepository : AbstractRepository<int, OutPatient>
    {
        public OutPatientRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<OutPatient> Delete(int key)
        {
            try
            {
                var outPatient = await Get(key);
                _context.Remove(outPatient);
                await _context.SaveChangesAsync();
                return outPatient;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("OutPatient");
            }

        }

        public override async Task<OutPatient> Get(int key)
        {
            var outPatient = await _context.OutPatients.Include(p=>p.Appointments).Include(p => p.Bills).SingleOrDefaultAsync(u => u.OutPatientId == key);
            if (outPatient != null)
            {
                return outPatient;
            }
            throw new ObjectNotAvailableException("OutPatient");
        }

        public override async Task<IEnumerable<OutPatient>> Get()
        {
            var outPatients = await _context.OutPatients.Include(p => p.Appointments).Include(p => p.Bills).ToListAsync();
            return outPatients;
        }

        public override async Task<OutPatient> Update(OutPatient item)
        {
            try
            {
                if (await Get(item.OutPatientId) != null)
                {
                    _context.Entry<OutPatient>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("OutPatient");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("OutPatient");
            }

        }
    }
}
