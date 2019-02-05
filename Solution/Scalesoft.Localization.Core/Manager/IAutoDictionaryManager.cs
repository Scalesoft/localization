using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Manager
{
    public interface IAutoDictionaryManager
    {
        IDictionary<string, LocalizedString> GetDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        IDictionary<string, PluralizedString> GetPluralizedDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        IDictionary<string, ClientPluralizedString> GetClientPluralizedDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        IDictionary<string, LocalizedString> GetConstantsDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null);

        CultureInfo GetDefaultCulture();
        CultureInfo[] GetSupportedCultures();

        void AddSingleDictionary(IDictionaryFactory dictionaryFactory, string filePath);

        void AddSingleDictionary(IDictionaryFactory dictionaryFactory, Stream resourceStream);
    }
}
