using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestConcepts.Domain.Interfaces;
using TestConcepts.Domain;
using TestConcepts.Business.Interfaces;

namespace TestConcepts.WebApi.Controllers
{
    /// <summary>
    /// Operations to do some calculations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;
        private readonly ICalculatorBusiness _calculatorDomain;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">Logger information about calculations.</param>
        /// <param name="calculator">Domain object to do the calcs.</param>
        public CalculatorController(ILogger<CalculatorController> logger, ICalculatorBusiness calculator)
        {
            _logger = logger;
            _calculatorDomain = calculator;
        }

        /// <summary>
        /// Do a division of two numbers like a/b.
        /// </summary>
        /// <param name="a">Decimal a.</param>
        /// <param name="b">Decimal b</param>
        /// <returns>Result of division.</returns>
        [HttpGet("Division")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDivisionAsync(decimal a, decimal b)
        {
            if (b == 0)
                return BadRequest("Can't not divided by zero.");

            var result = await _calculatorDomain.DivisionAsync(a, b);

            if (result != null) return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// Do a multiplication of two numbers like a/b.
        /// </summary>
        /// <param name="a">Decimal a.</param>
        /// <param name="b">Decimal b</param>
        /// <returns>Result of multiplication.</returns>
        [HttpGet("Multiplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMultiplicationAsync(decimal a, decimal b)
        {
            var result = await _calculatorDomain.MultiplicationAsync(a, b);

            if (result != null) return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// Do a subtraction of two numbers like a/b.
        /// </summary>
        /// <param name="a">Decimal a.</param>
        /// <param name="b">Decimal b</param>
        /// <returns>Result of subtraction.</returns>
        [HttpGet("Subtraction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSubtractionAsync(decimal a, decimal b)
        {
            var result = await _calculatorDomain.SubtractionAsync(a, b);

            if (result != null) return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// Do a sum of two numbers like a/b.
        /// </summary>
        /// <param name="a">Decimal a.</param>
        /// <param name="b">Decimal b</param>
        /// <returns>Result of sum.</returns>
        [HttpGet("Sum")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSumAsync(decimal a, decimal b)
        {
            var result = await _calculatorDomain.SumAsync(a, b);

            if (result != null) return NoContent();

            return Ok(result);
        }
    }
}
