using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationCore.Entities.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Calculator.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // use in-memory database
            ConfigureInMemoryDatabases(services);

            // use real database
            ConfigureProductionServices(services);
        }

        public void ConfigureDockerServices(IServiceCollection services)
        {
            ConfigureDevelopmentServices(services);
        }

        private void ConfigureInMemoryDatabases(IServiceCollection services)
        {
            //services.AddDbContext<TestConceptsContext>(c =>
            //    c.UseInMemoryDatabase("Catalog"));

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<TestConceptsContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("CalculatorConnection")));

            ConfigureServices(services);
        }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            ConfigureInMemoryDatabases(services);
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddMemoryCache();

            services.AddControllers();

            services.AddCustomDbContext(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<TestConceptsContext>(options =>
                    {
                        options.UseSqlServer(configuration["ConnectionString"],
                                             sqlServerOptionsAction: sqlOptions =>
                                             {
                                                 sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                                 sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                             });
                    });

            return services;
        }
    }
}
