using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class AdmissionRepository:AbstractRepository<int, Admission>
    {
        public AdmissionRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Admission> Delete(int key)
        {
            try
            {
                var admission = await Get(key);
                _context.Remove(admission);
                await _context.SaveChangesAsync();
                return admission;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Admission");
            }

        }

        public override async Task<Admission> Get(int key)
        {
            var admission = await _context.Admissions.Where(a => a.DoctorId == null || a.DoctorId != null).Include(p => p.AdmissionDetails).SingleOrDefaultAsync(u => u.AdmissionId == key);
            if (admission != null)
            {
                return admission;
            }
            throw new ObjectNotAvailableException("Admission");
        }

        public override async Task<IEnumerable<Admission>> Get()
        {
            var admissions = await _context.Admissions.Where(a => a.DoctorId == null || a.DoctorId != null).Include(p => p.AdmissionDetails).ToListAsync();
            return admissions;
        }

        public override async Task<Admission> Update(Admission item)
        {
            try
            {
                if (await Get(item.AdmissionId) != null)
                {
                    _context.Entry<Admission>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("Admission");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Admission");
            }

        }
    }
}
