using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class UserLoginDetailsRepository : AbstractRepository<int, UserLoginDetails>
    {
        public UserLoginDetailsRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<UserLoginDetails> Delete(int key)
        {
            try
            {
                var user = await Get(key);
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("User");
            }

        }

        public override async Task<UserLoginDetails> Get(int key)
        {
            var user = await _context.UserLoginDetails.SingleOrDefaultAsync(u => u.UserId == key);
            if (user != null)
            {
                return user;
            }
            throw new ObjectNotAvailableException("User");
        }

        public override async Task<IEnumerable<UserLoginDetails>> Get()
        {
            var user = await _context.UserLoginDetails.ToListAsync();
            return user;
        }

        public override async Task<UserLoginDetails> Update(UserLoginDetails item)
        {
            try
            {
                if (await Get(item.UserId) != null)
                {
                    _context.Entry<UserLoginDetails>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync(true);
                    return item;
                }
                throw new ObjectNotAvailableException("User");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("User");
            }

        }
    }
}
