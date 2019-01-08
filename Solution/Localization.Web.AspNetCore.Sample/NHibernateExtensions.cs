using Localization.Database.NHibernate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace Localization.Web.AspNetCore.Sample
{
    /// <summary>
    /// Configure NHibernate and it's database connection
    /// </summary>
    public static class NHibernateExtensions
    {
        public static void AddNHibernate(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MainDatabase");
            
            var cfg = new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.Dialect<MsSql2012Dialect>();
                    db.Driver<SqlClientDriver>();
                    db.ConnectionString = connectionString;
                    db.ConnectionProvider<DriverConnectionProvider>();
                    //db.LogFormattedSql = true;
                    //db.LogSqlInConsole = true;
                });

            cfg.AddMapping(GetMappings());

            var sessionFactory = cfg.BuildSessionFactory();

            services.AddSingleton(cfg);
            services.AddSingleton(sessionFactory);
        }
        
        private static HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(NHibernateDatabaseConfiguration.GetMappings());

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }
    }
}
