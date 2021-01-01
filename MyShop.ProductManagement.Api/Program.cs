using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyShop.ProductManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();
                    var isLocalEnvironment = string.Equals(builtConfig["ASPNETCORE_ENVIRONMENT"], "Local", StringComparison.OrdinalIgnoreCase);
                    if (!isLocalEnvironment)
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        config.AddAzureKeyVault(
                            $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                            keyVaultClient,
                            new DefaultKeyVaultSecretManager());
                    }
                })
                .ConfigureLogging((context, builder) =>
                {
                    var isLocal = string.Equals("Local", context.HostingEnvironment.EnvironmentName, StringComparison.OrdinalIgnoreCase);
                    if (isLocal)
                    {
                        builder.AddConsole();
                    }
                    else
                    {
                        var instrumentationKey = context.Configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");
                        builder.AddApplicationInsights(instrumentationKey, options => { options.FlushOnDispose = true; });
                        builder.AddAzureWebAppDiagnostics();

                        builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("", LogLevel.Debug);
                        builder.AddFilter<Microsoft.Extensions.Logging.AzureAppServices.FileLoggerProvider>("", LogLevel.Debug);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    }
}
