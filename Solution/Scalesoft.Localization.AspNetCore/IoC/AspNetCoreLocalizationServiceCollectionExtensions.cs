using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.AspNetCore.Factory;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.AspNetCore.Middleware;
using Scalesoft.Localization.AspNetCore.Service;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.IoC;

namespace Scalesoft.Localization.AspNetCore.IoC
{
    public static class AspNetCoreLocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService<T>(
            this IServiceCollection services, LocalizationConfiguration configuration, IDatabaseConfiguration databaseConfiguration = null
        ) where T : class, ICookieConfigResolver
        {
            services.AddLocalizationCore(configuration, databaseConfiguration);

            services.TryAddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();
            services.TryAddSingleton<IRequestCultureManager, RequestCultureManager>();

            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<IRazorLocalizationService, RazorLocalizationService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddTransient<IDynamicTextService, DynamicTextService>();
            services.TryAddTransient<ICookieConfigResolver, T>();
        }

        public static IApplicationBuilder UseLocalization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LocalizationCookieMiddleware>();
        }
    }
}
