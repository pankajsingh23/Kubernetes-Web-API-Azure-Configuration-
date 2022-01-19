using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DockerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            CreateWebHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c =>
                {
                    c.AddJsonFile("appsettings-copy.json", optional: true, reloadOnChange: true);
                    //c.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
              .ConfigureAppConfiguration((hostingContext, config) => ConfigureAppConfigurations(config))
              .UseStartup<Startup>();
        }
        private static void ConfigureAppConfigurations(IConfigurationBuilder config)
        {
            if (!Convert.ToBoolean(Environment.GetEnvironmentVariable("ConfigFromLocal")))
            {
                config.AddAzureAppConfiguration(options =>
                {
                    options.Connect(Environment.GetEnvironmentVariable("AppConfigConnectionString"))
                     .ConfigureRefresh(refresh =>
                     {
                         refresh.Register(key: "refreshAll", refreshAll: true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(30));
                     })
                    .Select(KeyFilter.Any, Environment.GetEnvironmentVariable("ENVIRONMENT_TYPE"));
                    options.ConfigureKeyVault(kv =>
                    {
                        kv.SetCredential(new DefaultAzureCredential());
                    });
                }, true);
            }
        }
    }
}
