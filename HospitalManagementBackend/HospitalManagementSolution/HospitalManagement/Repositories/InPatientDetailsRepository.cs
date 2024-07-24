using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class InPatientDetailsRepository:AbstractRepository<int, InPatientDetails>
    {
        public InPatientDetailsRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<InPatientDetails> Delete(int key)
        {
            try
            {
                var inPatientDetail = await Get(key);
                _context.Remove(inPatientDetail);
                await _context.SaveChangesAsync();
                return inPatientDetail;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("InPatientDetail");
            }

        }

        public override async Task<InPatientDetails> Get(int key)
        {
            var inPatientDetail = await _context.InPatientDetails.Include(p => p.Room).Include(p => p.InPatient).SingleOrDefaultAsync(u => u.InPatientDetailsId == key);
            if (inPatientDetail != null)
            {
                return inPatientDetail;
            }
            throw new ObjectNotAvailableException("InPatientDetail");
        }

        public override async Task<IEnumerable<InPatientDetails>> Get()
        {
            var inPatientDetails = await _context.InPatientDetails.Include(p => p.Room).Include(p=>p.InPatient).ToListAsync();
            return inPatientDetails;
        }

        public override async Task<InPatientDetails> Update(InPatientDetails item)
        {
            try
            {
                if (await Get(item.InPatientDetailsId) != null)
                {
                    _context.Entry<InPatientDetails>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("InPatientDetail");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("InPatientDetail");
            }

        }
    }
}
