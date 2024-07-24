using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class DoctorRepository : AbstractRepository<int,Doctor>
    {
        public DoctorRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Doctor> Delete(int key)
        {
            try
            {
                var doctor = await Get(key);
                _context.Entry<Doctor>(doctor).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return doctor;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Doctor> Get(int key)
        {
            var doctor = await _context.Doctors.Include(d => d.Appointments).SingleOrDefaultAsync(d => d.DoctorId == key);
            if (doctor == null)
                throw new ObjectNotAvailableException("Doctor");
            return doctor;
        }

        public override async Task<IEnumerable<Doctor>> Get()
        {
            var doctors = await _context.Doctors.Include(d => d.Appointments).ToListAsync();
            return doctors;

        }

        public override async Task<Doctor> Update(Doctor item)
        {
            try
            {
                if (await Get(item.DoctorId) != null)
                {
                    _context.Entry<Doctor>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Doctor");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
