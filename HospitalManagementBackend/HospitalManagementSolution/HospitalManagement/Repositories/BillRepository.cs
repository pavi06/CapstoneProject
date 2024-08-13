using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class BillRepository:AbstractRepository<int, Bill>
    {
        public BillRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Bill> Delete(int key)
        {
            try
            {
                var bill = await Get(key);
                _context.Entry<Bill>(bill).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return bill;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Bill> Get(int key)
        {
            var bill = await _context.Bills.SingleOrDefaultAsync(b=>b.BillId == key);
            if (bill == null)
                throw new ObjectNotAvailableException("Bill");
            return bill;
        }

        public override async Task<IEnumerable<Bill>> Get()
        {
            var bills = await _context.Bills.ToListAsync();
            return bills;

        }

        public override async Task<Bill> Update(Bill item)
        {
            try
            {
                if (await Get(item.BillId) != null)
                {
                    _context.Entry<Bill>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Bill");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
