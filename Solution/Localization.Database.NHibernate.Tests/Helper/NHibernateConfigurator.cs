using System.Reflection;
using Localization.Database.NHibernate.Dao;
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
        private const string ConnectionString = "Data Source=:memory:;Version=3;New=True;";

        public static Configuration GetNHibernateConfigurator()
        {
            var configuration = new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.Dialect<SQLiteDialect>();
                    db.Driver<SQLite20Driver>();
                    db.ConnectionProvider<DriverConnectionProvider>();
                    db.ConnectionString = ConnectionString;
                })
                .SetProperty(Environment.CurrentSessionContextClass, "thread_static");

            configuration.AddMapping(GetMappings());

            BuildSchema(configuration);

            return configuration;
        }

        private static HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            var mappings = Assembly.GetAssembly(typeof(NHibernateDao)).GetExportedTypes();
            mapper.AddMappings(mappings);
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }

        private static void BuildSchema(Configuration configuration)
        {
            var schemaExport = new SchemaExport(configuration);
            schemaExport.SetOutputFile("create.sql");
            schemaExport.Create(true, true);
        }

        public static ISessionFactory GetSessionFactory()
        {
            var sessionFactory = GetSessionFactory(GetNHibernateConfigurator());

            return sessionFactory;
        }

        public static ISessionFactory GetSessionFactory(Configuration configuration)
        {
            var sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory;
        }
    }
}