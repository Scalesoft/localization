using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Localization.CoreLibrary.Util
{
    public interface IConfiguration
    {
        string BasePath();
        IImmutableList<CultureInfo> SupportedCultures();
        CultureInfo DefaultCulture();
        LocTranslateFallbackMode TranslateFallbackMode();
        bool AutoLoadResources();
        LocLocalizationResource FirstAutoTranslateResource();
    }
}