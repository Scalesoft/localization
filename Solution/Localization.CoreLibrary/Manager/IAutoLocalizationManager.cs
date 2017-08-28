using System.Globalization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface IAutoLocalizationManager
    {
        LocalizedString Translate(EnTranslationSource translationSource, string text, 
            CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateFormat(EnTranslationSource translationSource, string text, 
            object[] parameters, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslatePluralization(EnTranslationSource translationSource, string text, 
            int number, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateConstant(EnTranslationSource translationSource, string text, 
            CultureInfo cultureInfo = null, string scope = null);
    }
}