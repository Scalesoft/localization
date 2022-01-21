using System;
using System.IO;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.AspNetCore.IoC;
using Scalesoft.Localization.AspNetCore.Sample.CookiePrefs;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Util;
using Scalesoft.Localization.Database.NHibernate;

namespace Scalesoft.Localization.AspNetCore.Sample
{
    public class Startup
    {
        private readonly IConfiguration m_configuration;

        private IContainer m_container;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            m_configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var localizationConfiguration = m_configuration.GetSection("Localization").Get<LocalizationConfiguration>();
            var databaseConfiguration = new NHibernateDatabaseConfiguration();
            services.AddLocalizationService<CookiePrefsResolver>(localizationConfiguration, databaseConfiguration);

            // Add framework services.
            services.AddMvc()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory
                        .Create(type.Name, LocTranslationSource.File.ToString());
                });

            services.AddNHibernate(m_configuration);

            services.AddApplicationInsightsTelemetry();

            m_container = new Container().WithDependencyInjectionAdapter(services);

            return m_container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
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

            app.UseLocalization();

            app.UseRouting();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapControllerRoute("default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            var dictionaryFactory = app.ApplicationServices.GetService<IDictionaryFactory>();
            var dictionaryManager = app.ApplicationServices.GetService<IAutoDictionaryManager>();

            AddLocalizationDictionary(dictionaryManager, dictionaryFactory, "cs-CZ.json");
            AddLocalizationDictionary(dictionaryManager, dictionaryFactory, "en.json");
        }

        private void AddLocalizationDictionary(
            IAutoDictionaryManager dictionaryManager,
            IDictionaryFactory dictionaryFactory,
            string fileName
        )
        {
            var filePath = Path.Combine("OtherLocalization", fileName);

            dictionaryManager.AddSingleDictionary(dictionaryFactory, filePath);
        }
    }
}
