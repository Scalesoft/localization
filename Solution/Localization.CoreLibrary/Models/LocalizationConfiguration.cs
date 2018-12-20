using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Util;

namespace Localization.CoreLibrary.Models
{
    public class LocalizationConfiguration : ILocalizationConfiguration
    {
        public string BasePath { get; set; }

        public IReadOnlyList<CultureInfo> SupportedCultures { get; set; }

        public CultureInfo DefaultCulture { get; set; }

        public LocTranslateFallbackMode TranslateFallbackMode { get; set; }

        public bool AutoLoadResources { get; set; }

        public LocLocalizationResource FirstAutoTranslateResource { get; set; }
    }
}