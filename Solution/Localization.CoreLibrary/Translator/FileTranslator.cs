using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Translator
{
    public static class FileTranslator
    {
        public static LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.Translate(LocTranslationSource.File, text, cultureInfo, scope);
        }

        public static LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateFormat(LocTranslationSource.File, text, parameters, cultureInfo, scope);
        }

        public static LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslatePluralization(LocTranslationSource.File, text, number, cultureInfo, scope);
        }

        public static LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateConstant(LocTranslationSource.File, text, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Dictionary.GetDictionary(LocTranslationSource.File, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            return Localization.Dictionary.GetConstantsDictionary(LocTranslationSource.File, cultureInfo, scope);
        }

        public static Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            return Localization.Dictionary.GetPluralizedDictionary(LocTranslationSource.File, cultureInfo, scope);
        }       
    }
}