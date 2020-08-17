using System;
using System.Collections.Generic;
using System.Text;

namespace TestConcepts.Repository   
{
    public interface IContext<T>
    {
        public T Add(T item);
        public T Update(T item);
        public List<T> GetAll(T item);
        public T Get(int code);
        public T Delete(T item);
    }
}
