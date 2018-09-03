using System.IO;
using Localization.AspNetCore.Service.Extensions;
using Localization.AspNetCore.Service.Factory;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.Web.AspNetCore.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string databaseConnectionString = @"Server=localhost;Database=ITJakubWebDBLocalization;Trusted_Connection=True;";


            Localization.CoreLibrary.Localization.Init(
                @"localizationsettings.json", null,
                new JsonDictionaryFactory());

            AddLocalizationDictionary("cs-CZ.json");
            AddLocalizationDictionary("en.json");

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLocalizationService();

            services.AddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();

            // Add framework services.
            services.AddMvc()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory
                        .Create(type.Name, LocTranslationSource.File.ToString());
                });
            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            Localization.CoreLibrary.Localization.AttachLogger(loggerFactory);         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

           
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void AddLocalizationDictionary(string fileName)
        {
            var filePath = Path.Combine("OtherLocalization", fileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                Localization.CoreLibrary.Localization.AddSingleDictionary(JsonDictionaryFactory.FactoryInstance, fileStream);
            }
        }
    }
}
