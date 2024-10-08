﻿using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class PaymentRepository : AbstractRepository<int, Payment>
    {
        public PaymentRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Payment> Delete(int key)
        {
            try
            {
                var payment = await Get(key);
                _context.Entry<Payment>(payment).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return payment;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Payment> Get(int key)
        {
            var payment = await _context.Payments.SingleOrDefaultAsync(b => b.PaymentId == key);
            if (payment == null)
                throw new ObjectNotAvailableException("Payment");
            return payment;
        }

        public override async Task<IEnumerable<Payment>> Get()
        {
            var payments = await _context.Payments.ToListAsync();
            return payments;

        }

        public override async Task<Payment> Update(Payment item)
        {
            try
            {
                if (await Get(item.BillId) != null)
                {
                    _context.Entry<Payment>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Payment");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
