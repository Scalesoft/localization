using Localization.Web.AspNetCore.Sample.Configuration;
using Microsoft.Extensions.Configuration;

namespace Localization.Web.AspNetCore.Sample.Utils
{
    public static class ConfigurationExtensions
    {
        public static DatabaseServerType GetDatabaseServerType(this IConfiguration configuration)
        {
            return configuration.GetValue<DatabaseServerType>("DatabaseServer");
        }
    }
}