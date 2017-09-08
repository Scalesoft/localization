using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;

namespace Localization.CoreLibrary.Util
{
    public interface IConfiguration
    {
        string BasePath();
        IList<CultureInfo> SupportedCultures();
        CultureInfo DefaultCulture();
        LocTranslateFallbackMode TranslateFallbackMode();
        bool AutoLoadResources();
        LocLocalizationResource FirstAutoTranslateResource();
        string DbSource();
        string DbUser();
        string DbPassword();
    }
}