using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    public interface ILocalization
    {

        //Explicit calls
        LocalizedString Translate(string text, string scope, LocTranslationSource translationSource);
        LocalizedString TranslateFormat(string text, string[] parameters, string scope, LocTranslationSource translationSource);
        LocalizedString TranslatePluralization(string text, int number, string scope, LocTranslationSource translationSource);
        LocalizedString TranslateConstant(string text, string scope, LocTranslationSource translationSource);

        //Explicit calls and translationSource = LocTranslationSource.Auto
        LocalizedString Translate(string text, string scope);
        LocalizedString TranslateFormat(string text, string[] parameters, string scope);
        LocalizedString TranslatePluralization(string text, int number, string scope);
        LocalizedString TranslateConstant(string text, string scope);

        //Without scope
        LocalizedString Translate(string text, LocTranslationSource translationSource);
        LocalizedString TranslateFormat(string text, string[] parameters, LocTranslationSource translationSource);
        LocalizedString TranslatePluralization(string text, int number, LocTranslationSource translationSource);
        LocalizedString TranslateConstant(string text, LocTranslationSource translationSource);

        //Without scope and translationSource = LocTranslationSource.Auto
        LocalizedString Translate(string text);
        LocalizedString TranslateFormat(string text, string[] parameters);
        LocalizedString TranslatePluralization(string text, int number);
        LocalizedString TranslateConstant(string text);
    }
}
