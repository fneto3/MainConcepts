using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Model.Interface
{
    public interface ICalculator
    {
        decimal A { get; set; }

        decimal B { get; set; }

        decimal Result { get; set; }

        CalculatorType CalculatorType { get; set; }
    }
}
