using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestConcepts.Domain.Context.Interfaces;

namespace TestConcepts.Domain.Context
{
    public class Repository<T> : IRepository<T> where T : IBaseEntity
    {
        private Context _context;
        private IRepository<T> _entity;

        private Repository(Context context, IRepository<T> entity)
        {
            _context = context;
            _entity = entity;
        }

        public Task<T> Add(T item)
        {
            return _entity.Add(item);
        }

        public Task<T> Delete(T item)
        {
            return _entity.Delete(item);
        }

        public Task<T> Get(int code)
        {
            return _entity.Get(code);
        }

        public Task<List<T>> GetAll(T item)
        {
            return _entity.GetAll(item);
        }

        public Task<T> Update(T item)
        {
            return _entity.Update(item);
        }
    }
}
