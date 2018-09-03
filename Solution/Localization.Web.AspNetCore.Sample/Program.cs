using System.IO;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Localization.Web.AspNetCore.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseNLog()
                .Build();

            host.Run();
        }
    }
}
