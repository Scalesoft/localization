using System.Globalization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public abstract class ManagerBase
    {
        protected readonly ILocalizationConfiguration m_configuration;
        protected readonly ILogger m_logger;

        protected ManagerBase(ILocalizationConfiguration configuration, ILogger logger = null)
        {
            m_configuration = configuration;
            m_logger = logger;
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
            return m_configuration.DefaultCulture;
        }

        public string DefaultScope()
        {
            return m_configuration.DefaultScope;
        }
    }
}
