using Localization.Database.NHibernate.Repository;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.Database.NHibernate.IoC
{
    public static class IocRegistrationExtensions
    {
        public static void RegisterLocalizationDataEntitiesComponents(this IServiceCollection services)
        {
            services.AddSingleton<CultureHierarchyRepository>();
            services.AddSingleton<CultureRepository>();

            services.AddSingleton<CultureHierarchyUoW>();
            services.AddSingleton<CultureUoW>();
        }
    }
}
