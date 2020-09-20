using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.API.Job.Interface
{
    public interface IIntegrationEventService
    {
        Task CheckIntegrationEventsJob();
    }
}
