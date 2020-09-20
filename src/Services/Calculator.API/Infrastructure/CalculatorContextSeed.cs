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
        public static async Task SeedAsync(CalculatorContext calculatorContext, ILogger<CalculatorContextSeed> logger, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
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
                    logger.LogError(ex.Message);
                    await SeedAsync(calculatorContext, logger, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<Model.CalculatorType> GetPreconfiguredCalculatorType()
        {
            return new List<Model.CalculatorType>()
            {
                new Model.CalculatorType { Name = "Addition" },
                new Model.CalculatorType { Name = "Subtraction" },
                new Model.CalculatorType { Name = "Division" },
                new Model.CalculatorType { Name = "Multiplication" }
            };
        }
    }
}
