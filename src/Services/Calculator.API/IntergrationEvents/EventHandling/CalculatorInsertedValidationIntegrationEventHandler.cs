using Calculator.API.Infrastructure;
using Calculator.API.IntergrationEvents.Events;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.API.IntergrationEvents.EventHandling
{
    public class CalculatorInsertedValidationIntegrationEventHandler
        : IIntegrationEventHandler<CalculatorInsertedEvent>
    {
        private readonly CalculatorContext _calculatorContext;
        private readonly ICalculatorIntegrationEventService _calculatorIntegrationEventService;
        private readonly ILogger<CalculatorInsertedValidationIntegrationEventHandler> _logger;

        public CalculatorInsertedValidationIntegrationEventHandler(
                 CalculatorContext catalogContext,
                 ICalculatorIntegrationEventService catalogIntegrationEventService,
                 ILogger<CalculatorInsertedValidationIntegrationEventHandler> logger)
        {
            _calculatorContext = catalogContext;
            _calculatorIntegrationEventService = catalogIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(CalculatorInsertedEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-Calculator"))
            {
                _logger.LogInformation("Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Calculator", @event);
            }
        }
    }
}
