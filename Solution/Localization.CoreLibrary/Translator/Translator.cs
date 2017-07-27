using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager.Impl;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Translator
{
    //TODO
    public class Translator
    {
        public static LocalizedString Translate(string text, CultureInfo cultureInfo, string scope = null)
        {
            return LocalizationManager.Instance.Translate(text, scope, cultureInfo);
        }

        public static LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo, string scope = null)
        {
            return LocalizationManager.Instance.TranslateFormat(text, parameters, scope);
        }

        public static HashSet<LocalizedString> GetDictionary(CultureInfo cultureInfo = null)
        {
            return DictionaryManager.Instance.GetDictionary(cultureInfo);
        }

        public static HashSet<LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null)
        {
            return DictionaryManager.Instance.GetDictionaryPart(part, cultureInfo);
        }


        //Add translation for SQL data
    }
}