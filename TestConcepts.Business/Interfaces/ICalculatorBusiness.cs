using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestConcepts.Domain.Interfaces;

namespace TestConcepts.Business.Interfaces
{
    public interface ICalculatorBusiness
    {
        Task<ICalculator> DivisionAsync(decimal a, decimal b);

        Task<ICalculator> MultiplicationAsync(decimal a, decimal b);

        Task<ICalculator> SubtractionAsync(decimal a, decimal b);

        Task<ICalculator> SumAsync(decimal a, decimal b);
    }
}
