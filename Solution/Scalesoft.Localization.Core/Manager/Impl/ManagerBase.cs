using System.Globalization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public abstract class ManagerBase
    {
        protected readonly LocalizationConfiguration m_configuration;
        protected readonly ILogger m_logger;

        protected ManagerBase(LocalizationConfiguration configuration, ILogger logger = null)
        {
            m_configuration = configuration;
            m_logger = logger;
        }

        protected CultureInfo CultureInfoNullCheck(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                return GetDefaultCulture();
            }

            return cultureInfo;
        }

        protected string ScopeNullCheck(string scope)
        {
            if (string.IsNullOrEmpty(scope))
            {
                return GetDefaultScope();
            }

            return scope;
        }

        public CultureInfo GetDefaultCulture()
        {
            return m_configuration.DefaultCulture;
        }

        public string GetDefaultScope()
        {
            return m_configuration.DefaultScope;
        }
    }
}
