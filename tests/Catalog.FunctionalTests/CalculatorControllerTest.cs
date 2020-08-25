using Calculator.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace Catalog.FunctionalTests
{
    public class CalculatorControllerTest
    {
        private readonly DbContextOptions<CalculatorContext> _dbOptions;

        public CalculatorControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<CalculatorContext>()
                //.UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new CalculatorContext(_dbOptions))
            {
                //dbContext.AddRange(GetFakeCalculators());
                dbContext.SaveChanges();
            }
        }

        [Fact]
        public void Get_calculator_itens()
        {
            //Arrange
            var typesFilterApplied = 1;
            var pageSize = 2;
            var pageIndex = 1;

            var expectedItemsInPage = 2;
            var expectedTotalItems = 3;
                
            var catalogContext = new CalculatorContext(_dbOptions);
            
            //var integrationServicesMock = new Mock<ICatalogIntegrationEventService>();

            ////Act
            //var orderController = new CatalogController(catalogContext, catalogSettings, integrationServicesMock.Object);
            //var actionResult = await orderController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

            ////Assert
            //Assert.IsType<ActionResult<PaginatedItemsViewModel<CatalogItem>>>(actionResult);
            //var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<CatalogItem>>(actionResult.Value);
            //Assert.Equal(expectedTotalItems, page.Count);
            //Assert.Equal(pageIndex, page.PageIndex);
            //Assert.Equal(pageSize, page.PageSize);
            //Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        //private List<Calculator> GetFakeCalculators()
        //{
        //    return new List<Calculator>()
        //    {
        //        new Calculator(1, 1, CalculatorTypes.Addition),
        //        new Calculator(2, 2, CalculatorTypes.Addition),
        //        new Calculator(33, 11, CalculatorTypes.Addition),
        //        new Calculator(2, 2, CalculatorTypes.Subtraction),
        //        new Calculator(12, 2, CalculatorTypes.Subtraction),
        //        new Calculator(21, 21, CalculatorTypes.Subtraction),
        //        new Calculator(10, 23, CalculatorTypes.Multiplication),
        //        new Calculator(133, 22, CalculatorTypes.Multiplication),
        //        new Calculator(1444, 22, CalculatorTypes.Multiplication),
        //        new Calculator(1566, 727, CalculatorTypes.Division),
        //        new Calculator(871, 268, CalculatorTypes.Division),
        //        new Calculator(961, 288, CalculatorTypes.Division),
        //    };
        //}
    }
}