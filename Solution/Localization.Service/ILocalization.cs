using Microsoft.Extensions.Localization;
using System;

namespace Localization.Service
{
    public interface ILocalization
    {
        LocalizedString Translate(string text, string scope = null);

        LocalizedString TranslateFormat(string text, object[] parameters, string scope = null);

        LocalizedString TranslatePluralization(string text, int number, string scope = null);

        LocalizedString TranslateConstant(string text, string scope = null);
    }
}
