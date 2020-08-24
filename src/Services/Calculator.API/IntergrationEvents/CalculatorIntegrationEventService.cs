using EventBus.Abstractions;
using EventBus.Events;
using Infrastructure.Data;
using IntegrationEventLogEF.Services;
using IntegrationEventLogEF.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.API.IntergrationEvents
{
    public class CalculatorIntegrationEventService : ICalculatorIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly CalculatorContext _calculatorContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<CalculatorIntegrationEventService> _logger;

        public CalculatorIntegrationEventService(
            ILogger<CalculatorIntegrationEventService> logger,
            IEventBus eventBus,
            CalculatorContext catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _calculatorContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_calculatorContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, "Calculator", evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, "Calculator", evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndCalculatorContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            await ResilientTransaction.New(_calculatorContext).ExecuteAsync(async () =>
            {
                await _calculatorContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _calculatorContext.Database.CurrentTransaction);
            });
        }
    }
}
