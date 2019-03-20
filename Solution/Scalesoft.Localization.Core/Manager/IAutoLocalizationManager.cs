using System;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Manager
{
    public interface IAutoLocalizationManager
    {
        LocalizedString Translate(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null);

        [Obsolete("Use new method with params")]
        LocalizedString TranslateFormat(LocTranslationSource translationSource, string text,
            object[] parameters, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateFormat(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null, params object[] parameters);
        
        LocalizedString TranslatePluralization(LocTranslationSource translationSource, string text,
            int number, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateConstant(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null);

        CultureInfo GetDefaultCulture();
        CultureInfo[] GetSupportedCultures();
    }
}