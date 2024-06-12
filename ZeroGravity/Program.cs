using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZeroGravity.Infrastructure;
using Azure.Identity;

namespace ZeroGravity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ToDo: beim Erstellen von neuen Migrations sollte der Teil auskommentiert werden und
            // ToDo: CreateHostBuilder(args).Build().Run() einkommentiert werden
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                //.AddJsonFile("appsettings.Production.json", false, true)
                .AddJsonFile("appsettings.json", false)
                //.AddJsonFile("appsettings.Development.json", true)
                //.AddJsonFile($"appsettings.{environmentName}.{Environment.MachineName}.json", true)
                .Build();

            var commonConfig = config.GetSection("Common").Get<CommonConfig>();
            var hostname = commonConfig?.Hostname;

            var builder = new WebHostBuilder();

            if (!string.IsNullOrEmpty(hostname))
            {
                builder.UseUrls(hostname);
            }
            else
            {
                // applicationUrl from launchSettings.json will be used
            }

#if DEBUG

            var host = builder
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();

#else
            // Release/Production
            CreateHostBuilder(args).Build().Run();
#endif

            // ToDo: Einkommentieren, wenn neue Migration erstellt wird
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    var keyVaultEndpoint = new Uri("https://mibokovault.vault.azure.net/"); // Environment.GetEnvironmentVariable("VaultUri")
                //    config.AddAzureKeyVault(
                //    keyVaultEndpoint,
                //    new DefaultAzureCredential());
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}