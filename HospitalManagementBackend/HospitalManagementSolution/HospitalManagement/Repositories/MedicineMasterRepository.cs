using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class MedicineMasterRepository : AbstractRepository<int, MedicineMaster>
    {
        public MedicineMasterRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<MedicineMaster> Delete(int key)
        {
            try
            {
                var record = await Get(key);
                _context.Entry<MedicineMaster>(record).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return record;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<MedicineMaster> Get(int key)
        {
            var record = await _context.MedicineMaster.SingleOrDefaultAsync(m => m.MedicineId == key);
            if (record == null)
                throw new ObjectNotAvailableException("MedicineMaster");
            return record;
        }

        public override async Task<IEnumerable<MedicineMaster>> Get()
        {
            var records = await _context.MedicineMaster.ToListAsync();
            return records;

        }

        public override async Task<MedicineMaster> Update(MedicineMaster item)
        {
            try
            {
                if (await Get(item.MedicineId) != null)
                {
                    _context.Entry<MedicineMaster>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("MedicineMaster");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
