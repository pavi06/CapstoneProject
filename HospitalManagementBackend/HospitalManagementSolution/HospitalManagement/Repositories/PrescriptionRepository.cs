using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class PrescriptionRepository: AbstractRepository<int, Prescription>
    {
        public PrescriptionRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override Task<Prescription> Add(Prescription item)
        {
            if(_context.Prescriptions.Any(p=>p.PrescriptionFor == item.PrescriptionFor))
            {
                throw new ObjectAlreadyExistsException("Prescription");
            }
            return base.Add(item);
        }

        public override async Task<Prescription> Delete(int key)
        {
            try
            {
                var prescription = await Get(key);
                _context.Entry<Prescription>(prescription).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return prescription;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Prescription> Get(int key)
        {
            var prescription = await _context.Prescriptions.Include(d => d.Patient).Include(d => d.Doctor).Include(d=>d.Medications).SingleOrDefaultAsync(d => d.PrescriptionId == key);
            if (prescription == null)
                throw new ObjectNotAvailableException("Prescription");
            return prescription;
        }

        public override async Task<IEnumerable<Prescription>> Get()
        {
            var prescriptions = await _context.Prescriptions.Include(d => d.Patient).Include(d => d.Doctor).Include(d=>d.Medications).ToListAsync();
            return prescriptions;

        }

        public override async Task<Prescription> Update(Prescription item)
        {
            try
            {
                if (await Get(item.PrescriptionId) != null)
                {
                    _context.Entry<Prescription>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Prescription");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
