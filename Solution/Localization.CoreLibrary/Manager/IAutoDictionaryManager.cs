﻿using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Localization.CoreLibrary.Manager
{
    public interface IAutoDictionaryManager
    {
        Dictionary<string, LocalizedString> GetDictionary(LocTranslationSource translationSource, 
            CultureInfo cultureInfo = null, string scope = null);

        Dictionary<string, PluralizedString> GetPluralizedDictionary(LocTranslationSource translationSource, 
            CultureInfo cultureInfo = null, string scope = null);

        Dictionary<string, LocalizedString> GetConstantsDictionary(LocTranslationSource translationSource, 
            CultureInfo cultureInfo = null, string scope = null);

        CultureInfo DefaultCulture();
    }
}