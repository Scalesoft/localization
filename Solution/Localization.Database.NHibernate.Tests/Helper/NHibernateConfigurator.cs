using Localization.Database.NHibernate.Provider;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Localization.Database.NHibernate.Tests.Helper
{
    public static class NHibernateConfigurator
    {
        private const string ConnectionString = "Data Source=db-{0}.db;Version=3";

        public static Configuration GetNHibernateConfigurator(string databaseFileName)
        {
            var configuration = new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.LogSqlInConsole = true;
                    db.LogFormattedSql = true;
                    db.Dialect<SQLiteDialect>();
                    db.Driver<SQLite20Driver>();
                    db.ConnectionProvider<DriverConnectionProvider>();
                    db.ConnectionString = string.Format(ConnectionString, databaseFileName);
                })
                .SetProperty(Environment.CurrentSessionContextClass, "thread_static");

            configuration.AddMapping(GetMappings());

            BuildSchema(configuration);

            return configuration;
        }

        private static HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            var mappings = new LocalizationMappingProvider().GetMappings();
            mapper.AddMappings(mappings);
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }

        private static void BuildSchema(Configuration configuration)
        {
            var schemaExport = new SchemaExport(configuration);
            schemaExport.Drop(false, true);
            schemaExport.Create(false, true);
        }

        public static ISessionFactory GetSessionFactory(string databaseFileName)
        {
            var sessionFactory = GetSessionFactory(GetNHibernateConfigurator(databaseFileName));

            return sessionFactory;
        }

        public static ISessionFactory GetSessionFactory(Configuration configuration)
        {
            var sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory;
        }
    }
}
