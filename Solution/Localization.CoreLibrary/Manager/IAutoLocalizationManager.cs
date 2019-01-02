using System.Globalization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface IAutoLocalizationManager
    {
        LocalizedString Translate(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateFormat(LocTranslationSource translationSource, string text,
            object[] parameters, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslatePluralization(LocTranslationSource translationSource, string text,
            int number, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateConstant(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null);

        CultureInfo DefaultCulture();
        CultureInfo[] SupportedCultures();
    }
}