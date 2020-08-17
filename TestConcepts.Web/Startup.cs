using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Timeout;
using TestConcepts.Web.Services;
using TestConcepts.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using TestConcepts.Web.Options;
using Microsoft.Extensions.Hosting.Internal;

namespace TestConcepts.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            // Policies
            var policy = services.AddPolicyRegistry();
            var exceptionRetryPolicy = Policy<HttpResponseMessage>
                                            .Handle<OperationCanceledException>()
                                            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2));

            policy.Add("retryPolicy", exceptionRetryPolicy);

            // Creating configurations respecting ISP with options Pattern
            services.Configure<RegisterOptions>(Configuration
                                                    .GetSection(RegisterOptions.Register));

            services.AddScoped(item =>
                                    new HttpClient
                                    {
                                        BaseAddress = new Uri("http://localhost/api/")
                                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
