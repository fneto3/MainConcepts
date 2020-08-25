using System.Threading.Tasks;
using Calculator.API.Model.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        [Route("GetAll")]
        public async Task<ActionResult<ICalculator>> GetAll()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<ICalculator>> Get(int id)
        {
            return Ok();
        }
    }
}