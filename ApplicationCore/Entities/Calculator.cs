using ApplicationCore.Entities.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Calculator : BaseEntity, IAggregateRoot
    {
        public Calculator(decimal a, decimal b)
        {
            A = a;
            B = b;
        }

        public decimal A { get; private set; }

        public decimal B { get; private set; }
    }
}
