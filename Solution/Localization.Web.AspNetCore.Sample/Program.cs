using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Localization.Web.AspNetCore.Sample
{
    public static class Program
    {
        /// <summary>
        /// Entrypoint method
        /// </summary>
        /// <param name="args">Application arguments</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Configuration of runtime
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((buildContext, configuration) =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    configuration.AddJsonFile("modules.json", optional: true);
                    configuration.AddJsonFile($"modules.{environment}.json", optional: true);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseApplicationInsights()
                .Build();
        }
    }
}
