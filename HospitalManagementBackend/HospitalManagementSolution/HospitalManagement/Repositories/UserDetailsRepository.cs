using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class UserDetailsRepository:AbstractRepository<int, UserDetails>
    {
        public UserDetailsRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<UserDetails> Delete(int key)
        {
            try
            {
                var userDetail = await Get(key);
                _context.Remove(userDetail);
                await _context.SaveChangesAsync();
                return userDetail;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("UserDetail");
            }

        }

        public override async Task<UserDetails> Get(int key)
        {
            var userDetail = await _context.UserDetails.SingleOrDefaultAsync(u => u.UserId == key);
            if (userDetail != null)
            {
                return userDetail;
            }
            throw new ObjectNotAvailableException("UserDetail");
        }

        public override async Task<IEnumerable<UserDetails>> Get()
        {
            var userDetails = await _context.UserDetails.ToListAsync();
            return userDetails;
        }

        public override async Task<UserDetails> Update(UserDetails item)
        {
            try
            {
                if (await Get(item.UserId) != null)
                {
                    _context.Entry<UserDetails>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("UserDetail");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("UserDetail");
            }

        }

    }
}
