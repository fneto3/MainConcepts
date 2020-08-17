using System;
using System.Collections.Generic;
using System.Text;

namespace TestConcepts.Repository
{
    public class Context<T> : IContext<T>
    {
        public T Add(T item)
        {
            throw new NotImplementedException();
        }

        public T Delete(T item)
        {
            throw new NotImplementedException();
        }

        public T Get(int code)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll(T item)
        {
            throw new NotImplementedException();
        }

        public T Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
