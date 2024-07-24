using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class WardBedAvailabilityRepository:AbstractRepository<int, WardBedAvailability>
    {
        public WardBedAvailabilityRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<WardBedAvailability> Delete(int key)
        {
            try
            {
                var wardBedType = await Get(key);
                _context.Entry<WardBedAvailability>(wardBedType).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return wardBedType;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<WardBedAvailability> Get(int key)
        {
            var wardBedType = await _context.WardBedAvailabilities.SingleOrDefaultAsync(b => b.WardBedTypeId == key);
            if (wardBedType == null)
                throw new ObjectNotAvailableException("WardBed");
            return wardBedType;
        }

        public override async Task<IEnumerable<WardBedAvailability>> Get()
        {
            var wardBedTypes = await _context.WardBedAvailabilities.ToListAsync();
            return wardBedTypes;

        }

        public override async Task<WardBedAvailability> Update(WardBedAvailability item)
        {
            try
            {
                if (await Get(item.WardBedTypeId) != null)
                {
                    _context.Entry<WardBedAvailability>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("WardBed");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
