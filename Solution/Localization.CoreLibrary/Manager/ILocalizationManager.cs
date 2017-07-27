using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface ILocalizationManager
    {
        LocalizedString Translate(string text, string scope = null, CultureInfo cultureInfo = null);

        LocalizedString TranslateFormat(string text, string[] parameters, string scope = null,
            CultureInfo cultureInfo = null);       
    }
}