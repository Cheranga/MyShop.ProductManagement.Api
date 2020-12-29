using System;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MyShop.ProductManagement.Api.Configs;
using MyShop.ProductManagement.Api.DataAccess;
using MyShop.ProductManagement.Api.Services;

namespace MyShop.ProductManagement.Api
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
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "MyShop.ProductManagement.Api", Version = "v1"}); });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddScoped<IProductsService, ProductsService>();
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfig"));
            services.AddScoped(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<DatabaseConfig>>().Value;
                return config;
            });

            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
            services.AddMediatR(typeof(Startup).Assembly);

            services.AddLogging(builder =>
            {
                var isLocal = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Local", StringComparison.OrdinalIgnoreCase);
                if (isLocal)
                {
                    builder.AddConsole();
                }
                else
                {
                    var instrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
                    builder.AddApplicationInsights(instrumentationKey);
                    builder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Debug);
                    builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Debug);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || string.Equals(env.EnvironmentName, "Local", StringComparison.OrdinalIgnoreCase))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyShop.ProductManagement.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}