using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model = Calculator.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculator.API.Infrastructure
{
    public class CalculatorContextSeed
    {
        public static async Task SeedAsync(CalculatorContext calculatorContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                // TODO: Only run this if using a real database
                calculatorContext.Database.Migrate();
                if (!await calculatorContext.CalculatorTypes.AnyAsync())
                {
                    await calculatorContext.CalculatorTypes.AddRangeAsync(
                        GetPreconfiguredCalculatorType());

                    await calculatorContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CalculatorContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(calculatorContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<Model.CalculatorType> GetPreconfiguredCalculatorType()
        {
            return new List<Model.CalculatorType>()
            {
                new Model.CalculatorType("Addition"),
                new Model.CalculatorType("Subtraction"),
                new Model.CalculatorType("Division"),
                new Model.CalculatorType("Multiplication")
            };
        }
    }
}
