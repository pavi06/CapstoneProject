using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class UserRepository:AbstractRepository<int, User>
    {
        public UserRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<User> Delete(int key)
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

        public override async Task<User> Get(int key)
        {
            var userDetail = await _context.Users.SingleOrDefaultAsync(u => u.UserId == key);
            if (userDetail != null)
            {
                return userDetail;
            }
            throw new ObjectNotAvailableException("UserDetail");
        }

        public override async Task<IEnumerable<User>> Get()
        {
            var userDetails = await _context.Users.ToListAsync();
            return userDetails;
        }

        public override async Task<User> Update(User item)
        {
            try
            {
                if (await Get(item.UserId) != null)
                {
                    _context.Entry<User>(item).State = EntityState.Modified;
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
