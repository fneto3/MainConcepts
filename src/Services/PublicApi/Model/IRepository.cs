using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Model
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(string key);
        IEnumerable<string> GetKeys();
        Task<T> UpdateAsync(T item);
        Task<bool> DeleteAsync(string key);
    }
}
