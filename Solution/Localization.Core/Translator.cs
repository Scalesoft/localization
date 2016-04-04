using System.Collections.Generic;
using System.Globalization;

namespace Localization.Core
{
    public sealed class Translator
    {

        public static string Translate(string text, CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.Translate(text, cultureInfo);
        }

        public static IDictionary<string, string> GetTranslation(CultureInfo cultureInfo = null)
        {
            return LocalizationManager.Instance.GetTranslation(cultureInfo);
        }
    }
}
