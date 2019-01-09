using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Pluralization;

namespace Scalesoft.Localization.Core.Database
{
    public interface IDatabaseDictionaryService
    {
        IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope);

        IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope);

        IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope);
    }
}