using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestConcepts.Business.Interfaces;
using TestConcepts.Domain.Context;
using TestConcepts.Domain.Context.Interfaces;
using TestConcepts.Domain.Interfaces;
using TestConcepts.Repository;

namespace TestConcepts.Business
{
    public class CalculatorBusiness : ICalculatorBusiness
    {
        private IRepository<ICalculator> _calculatorRepository;
        private Context _context;

        public CalculatorBusiness(IRepository<ICalculator> calculatorRepository, Context context)
        {
            _calculatorRepository = calculatorRepository;
            _context = context;
        }

        #region Methods

        public async Task<ICalculator> DivisionAsync(decimal a, decimal b)
        {
            return await _calculatorRepository.Add(new Calculator(_context)
            {
                A = a,
                B = b,
                Result = a / b
            });
        }

        public async Task<ICalculator> MultiplicationAsync(decimal a, decimal b)
        {
            return await _calculatorRepository.Add(new Calculator(_context)
            {
                A = a,
                B = b,
                Result = a * b
            });
        }

        public async Task<ICalculator> SubtractionAsync(decimal a, decimal b)
        {
            return await _calculatorRepository.Add(new Calculator(_context)
            {
                A = a,
                B = b,
                Result = a - b
            });
        }

        public async Task<ICalculator> SumAsync(decimal a, decimal b)
        {
            return await _calculatorRepository.Add(new Calculator(_context)
            {
                A = a,
                B = b,
                Result = a + b
            });
        }

        #endregion
    }
}
