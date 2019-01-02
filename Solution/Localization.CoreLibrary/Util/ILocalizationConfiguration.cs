using System.Collections.Generic;
using System.Globalization;

namespace Localization.CoreLibrary.Util
{
    public interface ILocalizationConfiguration
    {
        string BasePath { get; }

        string DefaultScope { get; }

        IReadOnlyList<CultureInfo> SupportedCultures { get; }

        CultureInfo DefaultCulture { get; }

        LocTranslateFallbackMode TranslateFallbackMode { get; }

        bool AutoLoadResources { get; }

        LocLocalizationResource FirstAutoTranslateResource { get; }
    }
}
