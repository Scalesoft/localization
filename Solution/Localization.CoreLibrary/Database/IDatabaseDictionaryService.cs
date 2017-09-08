using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseDictionaryService
    {
        Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope);

        Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope);

        Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope);
    }
}