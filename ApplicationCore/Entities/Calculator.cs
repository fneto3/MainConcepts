using ApplicationCore.Entities.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Calculator : BaseEntity, ICalculator
    {
        private readonly ILogger<Calculator> _logger;

        public Calculator(decimal a, decimal b, CalculatorTypes calculatorTypes)
        {
            A = a;
            B = b;
            CalculatorType = calculatorTypes;
        }

        public decimal A { get; private set; }

        public decimal B { get; private set; }

        public CalculatorTypes CalculatorType { get; private set; }

        public decimal Result { get; private set; }

        public async Task Calculate()
        {
            switch (CalculatorType) 
            {
                case CalculatorTypes.Addition:
                    Result = A + B;
                    break;
                case CalculatorTypes.Subtraction:
                    Result = A - B;
                    break;
                case CalculatorTypes.Division:
                    Result = A / B;
                    break;
                case CalculatorTypes.Multiplication:
                    Result = A * B;
                    break;
                default:
                    Result = 0;
                    break;
            }
        }
    }

    public enum CalculatorTypes
    {
        Addition,
        Subtraction,
        Division,
        Multiplication
    }
}
