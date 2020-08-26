using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Calculator.API.Infrastructure;
using Calculator.API.IntergrationEvents;
using Calculator.API.IntergrationEvents.Events;
using Calculator.API.Model;
using Calculator.API.Model.Interface;
using Calculator.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model = Calculator.API.Model;

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
            var item = new Model.Calculator
            {
                A = a,
                B = b
            };

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)Model.CalculatorTypes.Addition);

            item.CalculatorType = type;

            item.Calculate();

            _calculatorContext.Calculators.Add(item);

            await _calculatorContext.SaveChangesAsync();

            var newCalculatorItem = new CalculatorInsertedEvent(item.Id, item.A, item.B, item.Result, item.CalculatorType?.Name);

            await _calculatorIntegrationEventService.SaveEventAndCalculatorContextChangesAsync(newCalculatorItem);

            await _calculatorIntegrationEventService.PublishThroughEventBusAsync(newCalculatorItem);

            return Ok(item);
        }

        [Route("Subtraction")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Subtraction(decimal a, decimal b)
        {
            var item = new Model.Calculator
            {
                A = a,
                B = b
            };

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)Model.CalculatorTypes.Subtraction);

            item.CalculatorType = type;

            item.Calculate();

            _calculatorContext.Calculators.Add(item);

            await _calculatorContext.SaveChangesAsync();

            return Ok(item);
        }

        [Route("Division")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ICalculator>> Division(decimal a, decimal b)
        {
            if (b == 0)
                return BadRequest("Can not divide by zero.");

            var item = new Model.Calculator
            {
                A = a,
                B = b
            };

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)Model.CalculatorTypes.Division);

            item.CalculatorType = type;

            item.Calculate();

            _calculatorContext.Calculators.Add(item);

            await _calculatorContext.SaveChangesAsync();

            return Ok(item);
        }

        [Route("Multiplication")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Multiplication(decimal a, decimal b)
        {
            var item = new Model.Calculator
            {
                A = a,
                B = b
            };

            var type = _calculatorContext.CalculatorTypes.FirstOrDefault(item => item.Id == (int)Model.CalculatorTypes.Multiplication);

            item.CalculatorType = type;

            item.Calculate();

            _calculatorContext.Calculators.Add(item);

            await _calculatorContext.SaveChangesAsync();

            return Ok(item);
        }

        [Route("items/calculatorType/{calcultorTypeId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Model.Calculator>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Model.Calculator>>> ItemsByCalculatorTypeIdAsync(int calculatorType, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<Model.Calculator>)_calculatorContext.Calculators;

            root = root.Where(item => item.CalculatorType.Id == calculatorType);

            var totalItems = await root.LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItemsViewModel<Model.Calculator>(pageIndex, pageSize, totalItems, itemsOnPage);
        }
    }
}
