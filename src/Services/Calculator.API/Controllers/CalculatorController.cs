using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.Business;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Interfaces;
using Calculator.API.IntergrationEvents;
using Calculator.API.IntergrationEvents.Events;
using Calculator.API.ViewModel;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Business = ApplicationCore.Business;
using Entities = ApplicationCore.Entities;

namespace Calculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;
        private readonly CalculatorContext _calculatorContext;
        private readonly ICalculatorIntegrationEventService _calculatorIntegrationEventService;

        public CalculatorController(ILogger<CalculatorController> logger, CalculatorContext context, ICalculatorIntegrationEventService calculatorIntegrationEventService)
        {
            _logger = logger;
            _calculatorContext = context;
            _calculatorIntegrationEventService = calculatorIntegrationEventService;
        }

        [Route("Addition")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Addition(decimal a, decimal b)
        {
            var item = new Business.Calculator(a, b, CalculatorTypes.Addition);

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Addition);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculators.Add(data);
            
            await _calculatorContext.SaveChangesAsync();

            var newCalculatorItem = new CalculatorInsertedEvent(data.Id, data);

            // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
            await _calculatorIntegrationEventService.SaveEventAndCalculatorContextChangesAsync(newCalculatorItem);

            // Publish through the Event Bus and mark the saved event as published
            await _calculatorIntegrationEventService.PublishThroughEventBusAsync(newCalculatorItem);

            return Ok(data);
        }

        [Route("Subtraction")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Subtraction(decimal a, decimal b)
        {
            var item = new Business.Calculator(a, b, CalculatorTypes.Subtraction);

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Subtraction);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculators.Add(data);

            await _calculatorContext.SaveChangesAsync();

            return Ok(data);
        }

        [Route("Division")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ICalculator>> Division(decimal a, decimal b)
        {
            if (b == 0)
                return BadRequest("Can not divide by zero.");

            var item = new Business.Calculator(a, b, CalculatorTypes.Division);

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Division);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculators.Add(data);

            await _calculatorContext.SaveChangesAsync();

            return Ok(data);
        }

        [Route("Multiplication")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Multiplication(decimal a, decimal b)
        {
            var item = new Business.Calculator(a, b, CalculatorTypes.Multiplication);

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Multiplication);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculators.Add(data);

            await _calculatorContext.SaveChangesAsync();

            return Ok(data);
        }

        [Route("items/calculatorType/{calcultorTypeId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Entities.Calculator>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Entities.Calculator>>> ItemsByCalculatorTypeIdAsync(int calculatorType, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<Entities.Calculator>)_calculatorContext.Calculators;

            root = root.Where(item => item.CalculatorType.Id == calculatorType);

            var totalItems = await root.LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItemsViewModel<Entities.Calculator>(pageIndex, pageSize, totalItems, itemsOnPage);
        }
    }
}
