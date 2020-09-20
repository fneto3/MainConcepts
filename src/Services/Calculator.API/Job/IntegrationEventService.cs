using Calculator.API.Infrastructure;
using Calculator.API.IntergrationEvents;
using Calculator.API.Job.Interface;
using EventBus.Abstractions;
using EventBus.Events;
using Hangfire;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator.API.Job
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly ILogger<IntegrationEventService> _logger;
        private readonly ICalculatorIntegrationEventService _calculatorIntegrationEventService;
        private readonly IIntegrationEventLogService _eventLogService;

        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public IntegrationEventService(
              ILogger<IntegrationEventService> logger
            , CalculatorContext context
            , IEventBus eventBus
            , Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory
            , ICalculatorIntegrationEventService calculatorIntegrationEventService)
        {
            _logger = logger;
            _calculatorIntegrationEventService = calculatorIntegrationEventService;
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventLogService = _integrationEventLogServiceFactory(context.Database.GetDbConnection());
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task CheckIntegrationEventsJob()
        {
            RecurringJob.AddOrUpdate(
                () => CheckIntegrationEvents()
                ,
                "*/1 * * * *");
        }

        public async Task CheckIntegrationEvents()
        {
            await semaphore.WaitAsync();

            IntegrationEventLogEntry processing = null;

            try
            {
                var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync();

                foreach (var logEvt in pendingLogEvents)
                {
                    processing = logEvt;

                    _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, "Calculator Job", logEvt.IntegrationEvent);

                    var type = Type.GetType(logEvt.EventTypeName);
                    var integrationEvent = (IntegrationEvent)JsonConvert.DeserializeObject(logEvt.Content, type);

                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(integrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
            }
            catch (Exception ex)
            {
                if (processing != null)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", processing.EventId, "Calculator Job");

                    await _eventLogService.MarkEventAsFailedAsync(processing.EventId);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
