using System.Collections.Generic;
using System.Globalization;

namespace Localization.Core
{
    public sealed class Translator
    {

        public static string Translate(string text, string scope = null, CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.Translate(text, scope, cultureInfo);
        }

        public static string TranslateFormat(string text, string[] parameters, string scope = null, CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.TranslateFormat(text, parameters, scope, cultureInfo);
        }

        public static IDictionary<string, string> GetTranslation(CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.GetTranslation(cultureInfo);
        }
    }
}
