using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.IntegrationEvents.Events
{
    public class CalculatorInsertedEvent : IntegrationEvent
    {
        public int Id { get; set; }

        public decimal A { get; set; }

        public decimal B { get; set; }

        public decimal Result { get; set; }

        public string Type { get; set; }

        public CalculatorInsertedEvent( int id,
                                        decimal a,
                                        decimal b,
                                        decimal result,
                                        string type)
        {
            Id = id;
            A = a;
            B = b;
            Result = result;
            Type = type;
        }
    }
}
