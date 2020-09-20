using System;
using System.Data.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Calculator.API.Infrastructure;
using Calculator.API.IntergrationEvents;
using Calculator.API.IntergrationEvents.EventHandling;
using Calculator.API.IntergrationEvents.Events;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.AspNetCore;
using HealthChecks.UI.Client;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Collections.Generic;
using Calculator.API.Extension;
using Calculator.API.Job;
using Calculator.API.Job.Interface;

namespace Calculator.API
{
    public class Startup
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service Collection of Application.</param>
        /// <returns>Service collection.</returns>
        public void ConfigureServices(IServiceCollection services)
        {
            // Core Features
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            ConfigureRabbitMQ(services);
            ConfigureSwagger(services);
            ConfigureIntegrationServices(services);
            ConfigureHealthCheck(services);
            ConfigureDBContexts(services);
            ConfigureHangFire(services);

            var container = new ContainerBuilder();
            container.Populate(services);

            services.AddControllers();
            services.AddOptions();
        }

        public void ConfigureHangFire(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("CalculatorConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(Configuration.GetConnectionString("CalculatorConnection"));
            });

            JobStorage.Current = new SqlServerStorage(Configuration.GetConnectionString("CalculatorConnection"));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        /// <summary>
        /// Configure Rabbit MQ Service, seting Host, User and Password.
        /// </summary>
        /// <param name="services">Service Collection of Application.</param>
        private void ConfigureRabbitMQ(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusRabbit:EventBusConnection"],
                    DispatchConsumersAsync = true,
                    Port = 5672
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusRabbit:EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusRabbit:EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusRabbit:EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusRabbit:EventBusPassword"];
                }

                var retryCount = 5;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
        }

        /// <summary>
        /// Configure swagger, setting the version and Title of Application.
        /// </summary>
        /// <param name="services">Service Collection of Application.</param>
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new OpenApiInfo() 
                    { 
                        Title = "Calculator API", 
                        Version = "v1" 
                    });
            });
        }

        /// <summary>
        /// Configure the integration service for Calculator, setting the handlers.
        /// </summary>
        /// <param name="services">Service Collection of Application.</param>
        private void ConfigureIntegrationServices(IServiceCollection services)
        {
            // Configuring the Integration Service that call the event bus.
            services.AddTransient<ICalculatorIntegrationEventService
                , CalculatorIntegrationEventService>();

            // Configuring the event log service.
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var retryCount = 5;

                return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, "Calculator", retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<CalculatorInsertedValidationIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventService, IntegrationEventService>();
        }

        /// <summary>
        /// Configure the health check page, checkng informations about this App, Rabbit and SQL Server
        /// </summary>
        /// <param name="services">Service Collection of Application.</param>
        private void ConfigureHealthCheck(IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();

            // Rota HC
            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(
                    Configuration["ConnectionString"],
                    name: "Calculator.Api-check",
                    tags: new string[] { "Calculator" })
                .AddRabbitMQ(
                        $"amqp://{Configuration["EventBusRabbit:EventBusUserName"]}:{Configuration["EventBusRabbit:EventBusPassword"]}@{Configuration["EventBusConnection"]}",
                        name: "calculator-rabbitmqbus-check",
                        tags: new string[] { "rabbitmqbus" });

            services.AddHealthChecksUI()
                    .AddInMemoryStorage();
        }

        /// <summary>
        /// Configure DBContexts to use on Calculator Context and Integration Event Log Context.
        /// </summary>
        /// <param name="services">Service Collection of Application.</param>
        private void ConfigureDBContexts(IServiceCollection services)
        {
            services.AddDbContext<CalculatorContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("CalculatorConnection")));

            services.AddDbContext<IntegrationEventLogContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("CalculatorConnection"), b => b.MigrationsAssembly("Calculator.API")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
                IApplicationBuilder app
              , IWebHostEnvironment env
              , ILoggerFactory log
              , IBackgroundJobClient backgroundJobs)
        {
            //Autofac configuration
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            // Hangfire
            app.UseStaticFiles();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                StatsPollingInterval = 60000,
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });
            app.UseHangfireServer();
            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            // Core features.
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            // Configuring swagger.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculator V1");
            });

            // Configuring Healthcheck, setting endpoints.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });

                endpoints.MapHangfireDashboard();
            });
            app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

            // Subscribing application to eventbus.
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<CalculatorInsertedEvent, CalculatorInsertedValidationIntegrationEventHandler>();
        }
    }
}
