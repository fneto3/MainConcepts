using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicApi.Infrasctructure.Repositories;
using PublicApi.Model;
using PublicApi.Model.Interface;
using StackExchange.Redis;
using static PublicApi.Model.Calculator;

namespace PublicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;
        private readonly IRepository<Calculator> _redis;

        public CalculatorController(ILogger<CalculatorController> logger, IRepository<Calculator> redis)
        {
            _logger = logger;
            _redis = redis;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ICalculator>>> GetAll(CalculatorTypes types, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var data = await _redis.GetAll(string.Empty);

            return Ok(data);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<ICalculator>> Get(int id)
        {
            var data = await _redis.GetAll(string.Empty);

            return Ok(data);
        }
    }
}