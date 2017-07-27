using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface IDictionaryManager
    {
        HashSet<LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null);

        HashSet<LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null);
    }
}