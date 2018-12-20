using System.IO;
using Localization.AspNetCore.Service;
using Localization.AspNetCore.Service.Extensions;
using Localization.AspNetCore.Service.Factory;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.Factory;
using Localization.Database.NHibernate.IoC;
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLocalizationService();
            services.RegisterNHibernateLocalizationComponents();

            services.AddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();
            services.AddScoped<DynamicText>();

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
            const string databaseConnectionString =
                @"Server=localhost;Database=LocalizationDatabaseEFCore;Trusted_Connection=True;";

            CoreLibrary.Localization.AttachLogger(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var databaseServiceFactory = app.ApplicationServices.GetService<DatabaseServiceFactory>();
            var jsonDictionaryFactory = app.ApplicationServices.GetService<JsonDictionaryFactory>();

            CoreLibrary.Localization.Init(
                "localizationsettings.json",
                databaseServiceFactory,
                jsonDictionaryFactory
            );
            AddLocalizationDictionary("cs-CZ.json");
            AddLocalizationDictionary("en.json");

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

            CoreLibrary.Localization.AddSingleDictionary(JsonDictionaryFactory.FactoryInstance, filePath);
        }
    }
}
