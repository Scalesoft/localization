using System;
using DryIoc.Facilities.NHibernate;
using DryIoc.Transactions;
using Localization.Database.NHibernate.Provider;
using Localization.Web.AspNetCore.Sample.Configuration;
using Localization.Web.AspNetCore.Sample.Utils;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;

namespace Scalesoft.HealthPlatform.WebHub
{
    /// <summary>
    /// Configure NHibernate and it's database connection
    /// </summary>
    public class NHibernateInstaller : INHibernateInstaller
    {
        private readonly IConfiguration m_configuration;

        public NHibernateInstaller(
            IConfiguration configuration
        )
        {
            m_configuration = configuration;
        }

        public bool IsDefault => true;

        public string SessionFactoryKey => "default";

        public Maybe<IInterceptor> Interceptor => Maybe.None<IInterceptor>();

        public Configuration Config
        {
            get
            {
                var connectionString = m_configuration.GetConnectionString("MainDatabase");
                var databaseServer = m_configuration.GetDatabaseServerType();

                var cfg = new Configuration()
                    .DataBaseIntegration(db =>
                    {
                        ConfigureDialectAndDriver(db, databaseServer);
                        db.ConnectionString = connectionString;
                        db.ConnectionProvider<DriverConnectionProvider>();
                        //db.LogFormattedSql = true;
                        //db.LogSqlInConsole = true;
                    });

                cfg.AddMapping(GetMappings());

                if (databaseServer == DatabaseServerType.PostgreSql)
                {
                    // Workaround for creating correct queries (native NHibernate's method supports this only for keywords)
                    QuoteTableAndColumns(cfg);
                }

                return cfg;
            }
        }

        private void ConfigureDialectAndDriver(IDbIntegrationConfigurationProperties db,
            DatabaseServerType databaseServer)
        {
            switch (databaseServer)
            {
                case DatabaseServerType.SqlServer:
                    db.Dialect<MsSql2012Dialect>();
                    db.Driver<SqlClientDriver>();
                    break;
                case DatabaseServerType.PostgreSql:
                    db.Dialect<PostgreSQL81Dialect>();
                    db.Driver<NpgsqlDriver>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseServer), databaseServer, null);
            }
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(new LocalizationMappingProvider().GetMappings());

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            return mapping;
        }

        private void QuoteTableAndColumns(Configuration configuration)
        {
            foreach (var cm in configuration.ClassMappings)
            {
                QuoteTable(cm.Table);
            }

            foreach (var cm in configuration.CollectionMappings)
            {
                QuoteTable(cm.Table);
            }
        }

        private void QuoteTable(Table table)
        {
            if (!table.IsQuoted)
            {
                table.IsQuoted = true;
            }

            foreach (var column in table.ColumnIterator)
            {
                if (!column.IsQuoted)
                {
                    column.IsQuoted = true;
                }
            }
        }

        public void Registered(ISessionFactory factory)
        {
        }

        public Configuration Deserialize()
        {
            return null;
        }

        public void Serialize(Configuration configuration)
        {
            // Not required
        }

        public void AfterDeserialize(Configuration configuration)
        {
            // Not required
        }
    }
}
