using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.Business;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
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
        private readonly MainConceptsContext _calculatorContext;

        public CalculatorController(ILogger<CalculatorController> logger, MainConceptsContext context)
        {
            _logger = logger;
            _calculatorContext = context;
        }

        [Route("Addition")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Addition(decimal a, decimal b)
        {
            _logger.LogInformation("Testando");
            _logger.LogWarning("Teste");
            _logger.LogCritical("Erro");
            _logger.LogDebug("Debug");

            var item = new Business.Calculator(a, b, CalculatorTypes.Addition);

            var type = _calculatorContext.CalculatorType.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Addition);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculator.Add(data);
            
            await _calculatorContext.SaveChangesAsync();
            
            return Ok(data);
        }

        [Route("Subtraction")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Subtraction(decimal a, decimal b)
        {
            var item = new Business.Calculator(a, b, CalculatorTypes.Subtraction);

            var type = _calculatorContext.CalculatorType.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Subtraction);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculator.Add(data);

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

            var type = _calculatorContext.CalculatorType.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Division);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculator.Add(data);

            await _calculatorContext.SaveChangesAsync();

            return Ok(data);
        }

        [Route("Multiplication")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<ICalculator>> Multiplication(decimal a, decimal b)
        {
            var item = new Business.Calculator(a, b, CalculatorTypes.Multiplication);

            var type = _calculatorContext.CalculatorType.FirstOrDefault(item => item.Id == (int)CalculatorTypes.Multiplication);

            var data = new Entities.Calculator
            {
                A = item.A,
                B = item.B,
                Result = item.Result,
                CalculatorType = type
            };

            _calculatorContext.Calculator.Add(data);

            await _calculatorContext.SaveChangesAsync();

            return Ok(data);
        }
    }
}
