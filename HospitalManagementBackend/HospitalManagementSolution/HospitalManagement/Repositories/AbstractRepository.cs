using HospitalManagement.Contexts;
using HospitalManagement.Interfaces;

namespace HospitalManagement.Repositories
{
    public abstract class AbstractRepository<K, T> : IRepository<K, T>
    {
        protected readonly HospitalManagementContext _context;
        public AbstractRepository(HospitalManagementContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public abstract Task<T> Delete(K key);

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> Get();

        public abstract Task<T> Update(T item);
    }
}
