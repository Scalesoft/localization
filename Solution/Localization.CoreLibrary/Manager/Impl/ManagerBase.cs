using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Util;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
namespace Localization.CoreLibrary.Manager.Impl
{
    internal class ManagerBase
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