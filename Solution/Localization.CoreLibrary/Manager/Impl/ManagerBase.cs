using System.Globalization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public abstract class ManagerBase
    {
        protected readonly ILocalizationConfiguration Configuration;
        protected readonly ILogger Logger;

        protected ManagerBase(ILocalizationConfiguration configuration, ILogger logger = null)
        {
            Configuration = configuration;
            Logger = logger;
        }

        protected CultureInfo CultureInfoNullCheck(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                return DefaultCulture();
            }

            return cultureInfo;
        }

        protected string ScopeNullCheck(string scope)
        {
            if (string.IsNullOrEmpty(scope))
            {
                return DefaultScope();
            }

            return scope;
        }

        public CultureInfo DefaultCulture()
        {
            return Configuration.DefaultCulture;
        }

        public string DefaultScope()
        {
            return Configuration.DefaultScope;
        }
    }
}
