using Microsoft.Extensions.Localization;
using Localization.CoreLibrary.Util;

namespace Localization.Service
{
    public interface ILocalization
    {
        LocalizedString Translate(string text, string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto);

        LocalizedString TranslateFormat(string text, string[] parameters, string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto);

        LocalizedString TranslatePluralization(string text, int number, string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto);

        LocalizedString TranslateConstant(string text, string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto);

        //Without scope
        LocalizedString Translate(string text, LocTranslationSource translationSource = LocTranslationSource.Auto);

        LocalizedString TranslateFormat(string text, string[] parameters, LocTranslationSource translationSource = LocTranslationSource.Auto);

        LocalizedString TranslatePluralization(string text, int number, LocTranslationSource translationSource = LocTranslationSource.Auto);

        LocalizedString TranslateConstant(string text, LocTranslationSource translationSource = LocTranslationSource.Auto);
    }
}
