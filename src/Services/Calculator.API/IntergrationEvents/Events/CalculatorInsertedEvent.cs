using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = ApplicationCore.Entities;

namespace Calculator.API.IntergrationEvents.Events
{
    public class CalculatorInsertedEvent : IntegrationEvent
    {
        public int CalculatorId { get; }
        public IEnumerable<Entities.Calculator> CalculatorItems { get; }

        public CalculatorInsertedEvent(int calculatorId,
            IEnumerable<Entities.Calculator> calculatorItems)
        {
            CalculatorId = calculatorId;
            CalculatorItems = calculatorItems;
        }

        public CalculatorInsertedEvent(int calculatorId, Entities.Calculator calculatorItems)
        {
            CalculatorId = calculatorId;
            CalculatorItems = new List<Entities.Calculator> { calculatorItems };
        }
    }
}
