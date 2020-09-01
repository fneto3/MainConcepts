using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Model.Interface
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll(string hashKey);
        Task<T> GetAsync(string hashKey, string key);
        IEnumerable<string> GetKeys();
        Task<T> UpdateAsync(T item);
        Task<bool> DeleteAsync(string key);
    }
}
