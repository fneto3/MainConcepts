using ApplicationCore.Entities.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Calculator : BaseEntity, IAggregateRoot
    {
        public Calculator()
        {

        }

        public decimal A { get; set; }

        public decimal B { get; set; }

        public decimal Result { get; set; }

        public CalculatorType CalculatorType { get; set; }
    }
}
