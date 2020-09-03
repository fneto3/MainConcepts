using Calculator.API.Model.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Model = Calculator.API.Model;

namespace Catalog.FunctionalTests
{
    public class CalculatorScenarios : CalculatorBase
    {
        [Fact]
        public async Task Post_post_additional_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                // Arrange
                var content = new StringContent(string.Empty, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(Post.Addition(1, 2), content);

                // Act
                var contentr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Model.Calculator>(contentr);

                // Assert
                response.EnsureSuccessStatusCode();
                result.A.ShouldBe(1);
                result.B.ShouldBe(1);
                result.Result.ShouldBe(3);
                result.Id.ShouldNotBeNull();
                result.CalculatorType.Id.ShouldBe(1);
                result.CalculatorType.Name.ShouldBe("Addition");
            }
        }

        [Fact]
        public async Task Post_post_subtraction_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                // Arrange
                var content = new StringContent(string.Empty, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(Post.Subtraction(10, 5), content);

                // Act
                var contentr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Model.Calculator>(contentr);

                // Assert
                response.EnsureSuccessStatusCode();
                result.A.ShouldBe(10);
                result.B.ShouldBe(5);
                result.Result.ShouldBe(5);
                result.Id.ShouldNotBeNull();
                result.CalculatorType.Id.ShouldBe(2);
                result.CalculatorType.Name.ShouldBe("Subtraction");
            }
        }

        [Fact]
        public async Task Post_post_division_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                // Arrange
                var content = new StringContent(string.Empty, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(Post.Division(10, 5), content);

                // Act
                var contentr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Model.Calculator>(contentr);

                // Assert
                response.EnsureSuccessStatusCode();
                result.A.ShouldBe(10);
                result.B.ShouldBe(5);
                result.Result.ShouldBe(2);
                result.Id.ShouldNotBeNull();
                result.CalculatorType.Id.ShouldBe(2);
                result.CalculatorType.Name.ShouldBe("Division");
            }
        }

        [Fact]
        public async Task Post_post_multiplication_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                // Arrange
                var content = new StringContent(string.Empty, UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(Post.Multiplication(10, 5), content);

                // Act
                var contentr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Model.Calculator>(contentr);

                // Assert
                response.EnsureSuccessStatusCode();
                result.A.ShouldBe(10);
                result.B.ShouldBe(5);
                result.Result.ShouldBe(2);
                result.Id.ShouldNotBeNull();
                result.CalculatorType.Id.ShouldBe(2);
                result.CalculatorType.Name.ShouldBe("Multiplication");
            }
        }
    }
}
