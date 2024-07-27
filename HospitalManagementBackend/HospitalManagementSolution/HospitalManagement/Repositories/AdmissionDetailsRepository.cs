using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class AdmissionDetailsRepository:AbstractRepository<int, AdmissionDetails>
    {
        public AdmissionDetailsRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<AdmissionDetails> Delete(int key)
        {
            try
            {
                var admissionDetail = await Get(key);
                _context.Remove(admissionDetail);
                await _context.SaveChangesAsync();
                return admissionDetail;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("AdmissionDetails");
            }

        }

        public override async Task<AdmissionDetails> Get(int key)
        {
            var admissionDetails = await _context.AdmissionDetails.Include(p => p.Room).Include(p => p.Admission).SingleOrDefaultAsync(u => u.AdmissionDetailsId == key);
            if (admissionDetails != null)
            {
                return admissionDetails;
            }
            throw new ObjectNotAvailableException("AdmissionDetails");
        }

        public override async Task<IEnumerable<AdmissionDetails>> Get()
        {
            var admissionDetails = await _context.AdmissionDetails.Include(p => p.Room).Include(p => p.Admission).ToListAsync();
            return admissionDetails;
        }

        public override async Task<AdmissionDetails> Update(AdmissionDetails item)
        {
            try
            {
                if (await Get(item.AdmissionDetailsId) != null)
                {
                    _context.Entry<AdmissionDetails>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("AdmissionDetails");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("AdmissionDetails");
            }

        }
    }
}
