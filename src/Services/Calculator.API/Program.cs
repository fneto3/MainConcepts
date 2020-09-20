using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Calculator.API.Infrastructure;
using Calculator.API.Job.Interface;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calculator.API
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public async static Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                           .ReadFrom.Configuration(Configuration)
                           .Enrich.WithProperty("Calculator API", "Serilog Web App Sample")
                           .CreateLogger();

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetService<ILogger<CalculatorContextSeed>>();

                try
                {
                    var calculatorContext = services.GetRequiredService<CalculatorContext>();
                    await CalculatorContextSeed.SeedAsync(calculatorContext, logger);

                    var integrationContext = services.GetRequiredService<IntegrationEventLogContext>();
                    integrationContext.Database.Migrate();

                    var jobs = services.GetRequiredService<IIntegrationEventService>();
                    await jobs.CheckIntegrationEventsJob();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            await host.RunAsync();
        }

        public static IHost BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutofacServiceProviderFactory()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                    webBuilder.UseKestrel();    
                })
                .Build();
    }

    public static class IHostBuilderExtension
    {
        public static IHostBuilder UseAutofacServiceProviderFactory(this IHostBuilder hostbuilder)
        {
            hostbuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            return hostbuilder;
        }
    }
}
