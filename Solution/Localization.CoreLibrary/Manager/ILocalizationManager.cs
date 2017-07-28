using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface ILocalizationManager
    {
        LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null, string part = null);

        LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null, string part = null);       
    }
}