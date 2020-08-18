using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.Interfaces
{
    public interface ICalculator
    {
        decimal A { get; }

        decimal B { get; }

        CalculatorTypes CalculatorType { get; }

        decimal Result { get; }
    }
}
