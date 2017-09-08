﻿using System.Collections.Generic;
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
            return Localization.Translator.Translate(LocTranslationSource.Auto, text, cultureInfo, scope);
        }

        public static LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateFormat(LocTranslationSource.Auto, text, parameters, cultureInfo,
                scope);
        }

        public static LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslatePluralization(LocTranslationSource.Auto, text, number, cultureInfo, scope);
        }

        public static LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return Localization.Translator.TranslateConstant(LocTranslationSource.Auto, text, cultureInfo, scope);
        }

        public static Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            Dictionary<string, LocalizedString> result = Localization.Dictionary.GetDictionary(LocTranslationSource.Auto, cultureInfo, scope);
            if (result == null)
            {
                result = new Dictionary<string, LocalizedString>();
            }

            return result;
        }

        public static Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            Dictionary<string, LocalizedString> result = Localization.Dictionary.GetConstantsDictionary(LocTranslationSource.Auto, cultureInfo, scope);
            if (result == null)
            {
                result = new Dictionary<string, LocalizedString>();
            }

            return result;
        }

        public static Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            Dictionary<string, PluralizedString> result = Localization.Dictionary.GetPluralizedDictionary(LocTranslationSource.Auto, cultureInfo, scope);
            if (result == null)
            {
                result = new Dictionary<string, PluralizedString>();
            }

            return result;
        }
    }
}