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

        public static IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionary<string, LocalizedString> result = Localization.Dictionary.GetDictionary(LocTranslationSource.File, cultureInfo, scope);
            if (result == null)
            {
                result = new Dictionary<string, LocalizedString>();
            }

            return result;
        }

        public static IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            IDictionary<string, LocalizedString> result = Localization.Dictionary.GetConstantsDictionary(LocTranslationSource.File, cultureInfo, scope);
            if (result == null)
            {
                result = new Dictionary<string, LocalizedString>();
            }

            return result;
        }

        public static IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            IDictionary<string, PluralizedString> result = Localization.Dictionary.GetPluralizedDictionary(LocTranslationSource.File, cultureInfo, scope);
            if (result == null)
            {
                result = new Dictionary<string, PluralizedString>();
            }

            return result;
        }       
    }
}