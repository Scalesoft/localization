using System.Collections.Generic;
using System.Globalization;

namespace Localization.CoreLibrary.Util
{
    public interface IConfiguration
    {
        string BasePath();
        IList<CultureInfo> SupportedCultures();
        CultureInfo DefaultCulture();

        string DbSource();
        string DbUser();
        string DbPass();
    }
}