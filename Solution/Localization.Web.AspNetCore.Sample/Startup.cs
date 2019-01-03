using System;
using System.IO;
using DryIoc;
using DryIoc.Facilities.AutoTx.Extensions;
using DryIoc.Facilities.NHibernate;
using DryIoc.Microsoft.DependencyInjection;
using Localization.AspNetCore.Service.IoC;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Scalesoft.HealthPlatform.WebHub;

namespace Localization.Web.AspNetCore.Sample
{
    public class Startup
    {
        private readonly IConfiguration m_configuration;

        private IContainer m_container;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            m_configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLocalizationService(
                m_configuration,
                "Localization"
            );

            services.RegisterLocalizationDataEntitiesComponents();
            services.RegisterNHibernateLocalizationComponents();

            // Add framework services.
            services.AddMvc()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory
                        .Create(type.Name, LocTranslationSource.File.ToString());
                });

            m_container = new Container().WithDependencyInjectionAdapter(
                services,
                throwIfUnresolved: type => type.Name.EndsWith("Controller")
            );

            m_container.Register<INHibernateInstaller, NHibernateInstaller>(Reuse.Singleton);

            m_container.AddAutoTx();
            m_container.AddNHibernate();

            return m_container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            m_container.ResetAutoTxActivityContext();

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
