using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Translator
{
    public static class Translator
    {
        public static LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.Translate(text, cultureInfo, scope);
        }

        public static LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateFormat(text, parameters, cultureInfo, scope);
        }

        public static LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslatePluralization(text, number, cultureInfo, scope);
        }

        public static LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateConstant(text, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Dictionary.GetDictionary(cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Dictionary.GetDictionaryPart(part, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            return Localization.Dictionary.GetConstantsDictionary(cultureInfo, scope);
        }

        public static Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            return Localization.Dictionary.GetPluralizedDictionary(cultureInfo, scope);
        }

        //Add translation for SQL data
    }
}