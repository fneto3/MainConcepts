using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using PublicApi.IntegrationEvents.Events;
using PublicApi.Model;
using PublicApi.Model.Interface;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.IntegrationEvents.EventHandling
{
    public class CalculatorAddedIntegrantionEventHandler : IIntegrationEventHandler<CalculatorInsertedEvent>
    {
        private readonly ILogger<CalculatorAddedIntegrantionEventHandler> _logger;
        private readonly IRepository<Calculator> _repository;

        public CalculatorAddedIntegrantionEventHandler(
            ILogger<CalculatorAddedIntegrantionEventHandler> logger
            ,IRepository<Calculator> repository
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(CalculatorInsertedEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Identifier}-PublicAPI"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Identifier, "PublicAPI", @event);

                await _repository.UpdateAsync(new Model.Calculator
                {
                    Id = @event.Id,
                    A = @event.A,
                    B = @event.B,
                    Result = @event.Result,
                    Type = @event.Type
                });
            }
        }
    }
}
