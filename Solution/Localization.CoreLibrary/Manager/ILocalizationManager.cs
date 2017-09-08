﻿using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface ILocalizationManager
    {
        LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null);

        CultureInfo DefaultCulture();

        string DefaultScope();
    }
}