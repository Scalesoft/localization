using System.Globalization;
using Localization.CoreLibrary.Util;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class ManagerBase
    {
        protected readonly IConfiguration Configuration;

        protected ManagerBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected CultureInfo CultureInfoNullCheck(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                return Configuration.DefaultCulture();
            }

            return cultureInfo;
        }

        protected string ScopeNullCheck(string scope)
        {
            if (string.IsNullOrEmpty(scope))
            {
                return Localization.DefaultScope;
            }

            return scope;
        }
    }
}