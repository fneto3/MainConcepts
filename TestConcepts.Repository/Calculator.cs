using System;
using System.Threading.Tasks;
using TestConcepts.Domain.Context;
using TestConcepts.Domain.Context.Interfaces;
using TestConcepts.Domain.Interfaces;

namespace TestConcepts.Repository
{
    public class Calculator : ICalculator, IBaseEntity
    {
        private Context _context;
        public Calculator(Context context)
        {
            _context = context;
        }

        #region Properties

        public long Id { get; set; }
        public decimal A { get; set; }
        public decimal B { get; set; }
        public decimal Result { get; set; }

        #endregion

        #region Methods

        public async Task<ICalculator> DivisionAsync(ICalculator calculator)
        {
            return calculator;
        }

        public async Task<ICalculator> MultiplicationAsync(ICalculator calculator)
        {
            return calculator;
        }

        public async Task<ICalculator> SubtractionAsync(ICalculator calculator)
        {
            return calculator;
        }

        public async Task<ICalculator> SumAsync(ICalculator calculator)
        {
            return calculator;
        }

        #endregion
    }
}
