using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Translator
{
    public static class DatabaseTranslator
    {
        public static LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.Translate(LocTranslationSource.Database, text, cultureInfo, scope);
        }

        public static LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateFormat(LocTranslationSource.Database, text, parameters, cultureInfo, scope);
        }

        public static LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslatePluralization(LocTranslationSource.Database, text, number, cultureInfo, scope);
        }

        public static LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateConstant(LocTranslationSource.Database, text, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            throw new NotImplementedException();
        }
    }
}