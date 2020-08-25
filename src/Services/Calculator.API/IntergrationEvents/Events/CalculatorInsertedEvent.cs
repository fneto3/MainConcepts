using EventBus.Events;
using System.Collections.Generic;

namespace Calculator.API.IntergrationEvents.Events
{
    public class CalculatorInsertedEvent : IntegrationEvent
    {
        public int CalculatorId { get; }
        public IEnumerable<Model.Calculator> CalculatorItems { get; }

        public CalculatorInsertedEvent(int calculatorId,
            IEnumerable<Model.Calculator> calculatorItems)
        {
            CalculatorId = calculatorId;
            CalculatorItems = calculatorItems;
        }

        public CalculatorInsertedEvent(int calculatorId, Model.Calculator calculatorItems)
        {
            CalculatorId = calculatorId;
            CalculatorItems = new List<Model.Calculator> { calculatorItems };
        }
    }
}
