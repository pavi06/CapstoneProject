using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class WardRoomsAvailabilityRepository:AbstractRepository<int, WardRoomsAvailability>
    {
        public WardRoomsAvailabilityRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<WardRoomsAvailability> Delete(int key)
        {
            try
            {
                var wardBedType = await Get(key);
                _context.Entry<WardRoomsAvailability>(wardBedType).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return wardBedType;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<WardRoomsAvailability> Get(int key)
        {
            var wardBedType = await _context.WardBedAvailabilities.SingleOrDefaultAsync(b => b.WardTypeId == key);
            if (wardBedType == null)
                throw new ObjectNotAvailableException("WardBed");
            return wardBedType;
        }

        public override async Task<IEnumerable<WardRoomsAvailability>> Get()
        {
            var wardBedTypes = await _context.WardBedAvailabilities.ToListAsync();
            return wardBedTypes;

        }

        public override async Task<WardRoomsAvailability> Update(WardRoomsAvailability item)
        {
            try
            {
                if (await Get(item.WardTypeId) != null)
                {
                    _context.Entry<WardRoomsAvailability>(item).State = EntityState.Modified;
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
