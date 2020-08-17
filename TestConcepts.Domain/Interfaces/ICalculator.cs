using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestConcepts.Domain.Context.Interfaces;

namespace TestConcepts.Domain.Interfaces
{
    public interface ICalculator : IBaseEntity
    {
        #region Properties

        public decimal A
        {
            get;
            set;
        }

        public decimal B
        {
            get;
            set;
        }

        public decimal Result
        {
            get;
            set;
        }

        #endregion

        //#region Methods

        //public Task<ICalculator> SumAsync(decimal a, decimal b);
        //public Task<ICalculator> SubtractionAsync(decimal a, decimal b);
        //public Task<ICalculator> DivisionAsync(decimal a, decimal b);
        //public Task<ICalculator> MultiplicationAsync(decimal a, decimal b);

        //#endregion
    }
}
