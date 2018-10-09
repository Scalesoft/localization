using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseDictionaryService
    {
        IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope);

        IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope);

        IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope);
    }
}