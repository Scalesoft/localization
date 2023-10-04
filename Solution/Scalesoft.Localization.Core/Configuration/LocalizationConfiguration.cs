using System.Collections.Generic;
using System.Globalization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Configuration
{
    public class LocalizationConfiguration
    {
        public string BasePath { get; set; }

        public string DefaultScope { get; set; } = "global";

        public IReadOnlyList<CultureInfo> SupportedCultures { get; set; }

        public CultureInfo DefaultCulture { get; set; }

        public LocTranslateFallbackMode TranslateFallbackMode { get; set; }

        public bool AutoLoadResources { get; set; }

        public LocLocalizationResource FirstAutoTranslateResource { get; set; }

        public bool SecureCookie { get; set; } = false;

        public bool IsContainerEnvironment { get; set; } = false;
    }
}
