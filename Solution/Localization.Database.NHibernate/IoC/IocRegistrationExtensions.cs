using Localization.Database.NHibernate.Repository;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.Database.NHibernate.IoC
{
    public static class IocRegistrationExtensions
    {
        public static void RegisterLocalizationDataEntitiesComponents(this IServiceCollection services)
        {
            services.AddSingleton<CultureRepository>();

            services.AddSingleton<CultureUoW>();
        }
    }
}