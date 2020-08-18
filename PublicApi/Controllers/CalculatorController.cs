using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicApi.Models.DTO;

namespace PublicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Addition")]
        public async Task<ActionResult<ICalculator>> Addition(decimal a, decimal b)
        {
            var item = new Calculator(a, b, CalculatorTypes.Addition);

            await item.Calculate();

            return Ok(item);
        }

        [HttpGet]
        [Route("Subtraction")]
        public async Task<ActionResult<ICalculator>> Subtraction(decimal a, decimal b)
        {
            var item = new Calculator(a, b, CalculatorTypes.Subtraction);

            await item.Calculate();

            return Ok(item);
        }

        [HttpGet]
        [Route("Division")]
        public async Task<ActionResult<ICalculator>> Division(decimal a, decimal b)
        {
            if (b == 0)
                return BadRequest("Can not divide by zero.");

            var item = new Calculator(a, b, CalculatorTypes.Division);

            await item.Calculate();

            return Ok(item);
        }

        [HttpGet]
        [Route("Multiplication")]
        public async Task<ActionResult<ICalculator>> Multiplication(decimal a, decimal b)
        {
            var item = new Calculator(a, b, CalculatorTypes.Multiplication);

            await item.Calculate();

            return Ok(item);
        }
    }
}