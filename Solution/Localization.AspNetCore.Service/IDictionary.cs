using System.Collections.Generic;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    public interface IDictionary
    {
        Dictionary<string, LocalizedString> GetDictionary(string scope = null, 
            LocTranslationSource translationSource = LocTranslationSource.Auto);

        Dictionary<string, PluralizedString> GetPluralizedDictionary(string scope = null, 
            LocTranslationSource translationSource = LocTranslationSource.Auto);

        Dictionary<string, LocalizedString> GetConstantsDictionary(string scope = null, 
            LocTranslationSource translationSource = LocTranslationSource.Auto);
    }
}