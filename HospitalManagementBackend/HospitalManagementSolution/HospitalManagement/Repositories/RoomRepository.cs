using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class RoomRepository:AbstractRepository<int, Room>
    {
        public RoomRepository(HospitalManagementContext context) : base(context)
        {
        }

        public override async Task<Room> Delete(int key)
        {
            try
            {
                var room = await Get(key);
                _context.Entry<Room>(room).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return room;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Room> Get(int key)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(r => r.RoomId == key);
            if (room == null)
                throw new ObjectNotAvailableException("Room");
            return room;
        }

        public override async Task<IEnumerable<Room>> Get()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return rooms;

        }

        public override async Task<Room> Update(Room item)
        {
            try
            {
                if (await Get(item.RoomId) != null)
                {
                    _context.Entry<Room>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Room");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
