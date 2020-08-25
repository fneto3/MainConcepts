using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Calculator.API.Model
{
    public class Calculator
    {
        public Calculator()
        {

        }

        public int Id { get; set; }

        public decimal A { get; set; }

        public decimal B { get; set; }

        public decimal Result { get; set; }

        public CalculatorType CalculatorType { get; set; }

        public void Calculate()
        {
            switch ((CalculatorTypes)CalculatorType.Id)
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
        Addition = 1,
        Subtraction = 2,
        Division = 3,
        Multiplication = 4
    }
}
