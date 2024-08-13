using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class MedicationRepository : AbstractRepository<int, Medication>
    {
        public MedicationRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Medication> Delete(int key)
        {
            try
            {
                var record = await Get(key);
                _context.Entry<Medication>(record).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return record;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Medication> Get(int key)
        {
            var record = await _context.Medications.SingleOrDefaultAsync(b => b.MedicationId == key);
            if (record == null)
                throw new ObjectNotAvailableException("Medication");
            return record;
        }

        public override async Task<IEnumerable<Medication>> Get()
        {
            var records = await _context.Medications.ToListAsync();
            return records;

        }

        public override async Task<Medication> Update(Medication item)
        {
            try
            {
                if (await Get(item.MedicationId) != null)
                {
                    _context.Entry<Medication>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Medication");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
