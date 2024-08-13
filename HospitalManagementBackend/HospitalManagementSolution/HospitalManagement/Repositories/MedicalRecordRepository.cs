using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class MedicalRecordRepository:AbstractRepository<int, MedicalRecord>
    {
        public MedicalRecordRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<MedicalRecord> Delete(int key)
        {
            try
            {
                var record = await Get(key);
                _context.Entry<MedicalRecord>(record).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return record;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<MedicalRecord> Get(int key)
        {
            var record = await _context.MedicalRecords.Include(r => r.Doctor).Include(r =>r.Medication).SingleOrDefaultAsync(b => b.RecordId == key);
            if (record == null)
                throw new ObjectNotAvailableException("MedicalRecord");
            return record;
        }

        public override async Task<IEnumerable<MedicalRecord>> Get()
        {
            var records = await _context.MedicalRecords.Include(r => r.Doctor).Include(r => r.Medication).ToListAsync();
            return records;

        }

        public override async Task<MedicalRecord> Update(MedicalRecord item)
        {
            try
            {
                if (await Get(item.RecordId) != null)
                {
                    _context.Entry<MedicalRecord>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("MedicalRecord");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
