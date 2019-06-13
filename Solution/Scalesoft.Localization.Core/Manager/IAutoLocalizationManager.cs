using System;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Manager
{
    public interface IAutoLocalizationManager
    {
        LocalizedString Translate(LocTranslationSource translationSource,
            CultureInfo cultureInfo, string scope, string text);

        LocalizedString TranslateFormat(LocTranslationSource translationSource,
            CultureInfo cultureInfo, string scope, string text,
            params object[] parameters);
        
        LocalizedString TranslatePluralization(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope, string text,
            int number);

        LocalizedString TranslateConstant(LocTranslationSource translationSource,
            CultureInfo cultureInfo, string scope, string text);

        CultureInfo GetDefaultCulture();
        CultureInfo[] GetSupportedCultures();
    }
}