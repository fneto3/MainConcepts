using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestConcepts.Domain.Context.Interfaces
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T> Add(T item);
        Task<T> Update(T item);
        Task<List<T>> GetAll(T item);
        Task<T> Get(int code);
        Task<T> Delete(T item);
    }
}
