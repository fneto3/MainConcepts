using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class TestConceptsContextSeed
    {
        public static async Task SeedAsync(TestConceptsContext calculatorContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                // TODO: Only run this if using a real database
                calculatorContext.Database.Migrate();
                if (!await calculatorContext.CalculatorType.AnyAsync())
                {
                    await calculatorContext.CalculatorType.AddRangeAsync(
                        GetPreconfiguredCalculatorType());

                    await calculatorContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<TestConceptsContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(calculatorContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<CalculatorType> GetPreconfiguredCalculatorType()
        {
            return new List<CalculatorType>()
            {
                new CalculatorType("Addition"),
                new CalculatorType("Subtraction"),
                new CalculatorType("Division"),
                new CalculatorType("Multiplication")
            };
        }
    }
}
