using Calculator.API;
using Calculator.API.Extension;
using Calculator.API.Infrastructure;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;

namespace Catalog.FunctionalTests
{
    public class CalculatorBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(CalculatorBase)).Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.Tests.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();

            var testServer = new TestServer(hostBuilder);

            testServer.Host
                .MigrateDbContext<CalculatorContext>((context, services) =>
                {
                    var env = services.GetService<IWebHostEnvironment>();
                    var logger = services.GetService<ILogger<CalculatorContextSeed>>();

                    CalculatorContextSeed
                        .SeedAsync(context, logger)
                        .Wait();
                })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

            return testServer;
        }

        public static class Post
        {
            public static string Addition(decimal a, decimal b)
            {
                return $"Calculator/Addition?a={a}&b={b}";
            }

            public static string Subtraction(decimal a, decimal b)
            {
                return $"Calculator/Subtraction?a={a}&b={b}";
            }

            public static string Division(decimal a, decimal b)
            {
                return $"Calculator/Division?a={a}&b={b}";
            }

            public static string Multiplication(decimal a, decimal b)
            {
                return $"Calculator/Multiplication?a={a}&b={b}";
            }
        }
    }
}
