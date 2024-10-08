﻿namespace HospitalManagement.Interfaces
{
    public interface IRepository<K,T>
    {
        public Task<T> Add(T item);
        public Task<T> Delete(K key);
        public Task<T> Update(T item);
        public Task<T> Get(K key);
        public Task<IEnumerable<T>> Get();
    }
}
