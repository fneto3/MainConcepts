using Calculator.API;
using Calculator.API.Controllers;
using Calculator.API.Infrastructure;
using Calculator.API.IntergrationEvents;
using Calculator.API.Model;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Calculator.UnitTests
{
    public class CalculatorControllerTest
    {
        private readonly DbContextOptions<CalculatorContext> _dbOptions;

        public CalculatorControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<CalculatorContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new CalculatorContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeCatalog());
                dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Get_CalculatorItems_Success()
        {
            //Arrange
            var calculatorContext = new CalculatorContext(_dbOptions);

            var integrationServicesMock = new Mock<ICalculatorIntegrationEventService>();
            var loggerMock = new Mock<ILogger<CalculatorController>>();

            //Act
            var calculatorController = new CalculatorController(loggerMock.Object, calculatorContext, integrationServicesMock.Object);
            var actionResult = await calculatorController.Addition(2, 5);

            //Assert

        }

        private List<API.Model.Calculator> GetFakeCatalog()
        {
            return new List<API.Model.Calculator>()
            {
                new API.Model.Calculator()
                {
                    Id = 1,
                    A = 2,
                    B = 4,
                    CalculatorType = new CalculatorType
                    {
                        Id = 1
                    },
                    Result = 6
                }
            };
        }
    }
}
