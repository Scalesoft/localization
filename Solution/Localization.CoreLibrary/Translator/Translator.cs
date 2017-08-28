using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Translator
{
    public static class Translator
    {

        public static LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.Translate(EnTranslationSource.Auto, text, cultureInfo, scope);
        }

        public static LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateFormat(EnTranslationSource.Auto, text, parameters, cultureInfo,
                scope);
        }

        public static LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslatePluralization(EnTranslationSource.Auto, text, number, cultureInfo, scope);
        }

        public static LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateConstant(EnTranslationSource.Auto, text, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            Dictionary<string, LocalizedString> result = Localization.Dictionary.GetDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                result = null; //TODO:
            }

            return result;
        }

        public static Dictionary<string, LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            Dictionary<string, LocalizedString> result = Localization.Dictionary.GetDictionaryPart(part, cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                result = null; //TODO:
            }

            return result;
        }

        public static Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            Dictionary<string, LocalizedString> result = Localization.Dictionary.GetConstantsDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                result = null; //TODO:
            }

            return result;
        }

        public static Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            Dictionary<string, PluralizedString> result = Localization.Dictionary.GetPluralizedDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                result = null; //TODO:
            }

            return result;
        }
    }
}