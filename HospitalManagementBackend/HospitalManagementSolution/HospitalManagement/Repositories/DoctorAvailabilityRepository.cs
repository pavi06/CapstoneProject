using HospitalManagement.Contexts;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class DoctorAvailabilityRepository : IRepositoryForCompositeKey<int,DateTime, DoctorAvailability>
    {
        protected readonly HospitalManagementContext _context;
        public DoctorAvailabilityRepository(HospitalManagementContext context)
        {
            _context = context;
        }

        public async Task<DoctorAvailability> Add(DoctorAvailability item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<DoctorAvailability> Delete(int key1, DateTime key2)
        {
            var doctorAvailability = await Get(key1, key2);
            if (doctorAvailability == null)
            {
                return null;
            }
            _context.Entry<DoctorAvailability>(doctorAvailability).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return doctorAvailability;

        }

        public async Task<DoctorAvailability> Get(int key1, DateTime key2)
        {
            var doctorAvailability = await _context.DoctorAvailability.SingleOrDefaultAsync(d => d.DoctorId == key1 && d.AppointmentDate == key2);
            return doctorAvailability;
        }

        public async Task<IEnumerable<DoctorAvailability>> Get()
        {
            var doctorAvailability = await _context.DoctorAvailability.ToListAsync();
            return doctorAvailability;
        }

        public async Task<DoctorAvailability> Update(DoctorAvailability item)
        {
            if (await Get(item.DoctorId, item.AppointmentDate) != null)
            {
                _context.Entry<DoctorAvailability>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
