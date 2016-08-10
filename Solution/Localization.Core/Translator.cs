using System.Collections.Generic;
using System.Globalization;

namespace Localization.Core
{
    public sealed class Translator
    {

        public static string Translate(string text, string scope = "", string[] parameters = null, CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.Translate(text, scope, parameters, cultureInfo);
        }

        public static string Translate(string text, string[] parameters, CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.Translate(text, parameters, cultureInfo);
        }

        public static IDictionary<string, string> GetTranslation(CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.GetTranslation(cultureInfo);
        }
    }
}
