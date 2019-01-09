using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.IO;
using Localization.CoreLibrary.Dictionary;

namespace Localization.CoreLibrary.Manager
{
    public interface IAutoDictionaryManager
    {
        IDictionary<string, LocalizedString> GetDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        IDictionary<string, PluralizedString> GetPluralizedDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        IDictionary<string, LocalizedString> GetConstantsDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        CultureInfo GetDefaultCulture();
        CultureInfo[] GetSupportedCultures();

        void AddSingleDictionary(IDictionaryFactory dictionaryFactory, string filePath);

        void AddSingleDictionary(IDictionaryFactory dictionaryFactory, Stream resourceStream);
    }
}
