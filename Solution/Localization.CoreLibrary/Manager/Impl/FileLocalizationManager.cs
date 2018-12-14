using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Localization.CoreLibrary.Manager.Impl
{
    internal class FileLocalizationManager : LocalizationManager, ILocalizationManager
    {
        private FileDictionaryManager m_dictionaryManager;

        public FileLocalizationManager(IConfiguration configuration) : base(configuration)
        {
            //Should be empty
        }

        public void AddDictionaryManager(IDictionaryManager dictionaryManager)
        {
            m_dictionaryManager = (FileDictionaryManager) dictionaryManager;
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            var ls = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
            ls.List().TryGetValue(text, out var result);
            if (result == null)
            {
                result = TranslateInParent(ls, text, cultureInfo, scope);
            }

            return result;
        }

        private LocalizedString TranslateInParent(ILocalizationDictionary localizationDictionary, string text,
            CultureInfo cultureInfo = null, string scope = null)
        {
            if (localizationDictionary.ParentDictionary() != null)
            {
                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.List().TryGetValue(text, out var result);

                if (result != null)
                {
                    return result;
                }

                return TranslateInParent(localizationDictionary, text, cultureInfo, scope);
            }

            return TranslateFallback(text);
        }

        private LocalizedString TranslateConstantInParent(ILocalizationDictionary localizationDictionary, string text,
            CultureInfo cultureInfo = null, string scope = null)
        {
            if (localizationDictionary.ParentDictionary() != null)
            {
                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.ListConstants().TryGetValue(text, out var result);

                if (result != null)
                {
                    return result;
                }

                return TranslateConstantInParent(localizationDictionary, text, cultureInfo, scope);
            }

            return TranslateFallback(text);
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            var nonParameterizedTranslation = Translate(text, cultureInfo, scope);
            var parametrizedTranslationString = string.Format(nonParameterizedTranslation.Value, parameters);

            return new LocalizedString(nonParameterizedTranslation.Name, parametrizedTranslationString);
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            var ls = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
            ls.ListPlurals().TryGetValue(text, out var resultPluralizedString);

            if (resultPluralizedString == null)
            {
                return TranslatePluralizedInParent(ls, text, number, cultureInfo, scope);
            }

            return resultPluralizedString.GetPluralizedLocalizedString(number);
        }

        private LocalizedString TranslatePluralizedInParent(ILocalizationDictionary localizationDictionary, string text,
            int number, CultureInfo cultureInfo = null, string scope = null)
        {
            if (localizationDictionary.ParentDictionary() != null)
            {
                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.ListPlurals().TryGetValue(text, out var result);

                if (result != null)
                {
                    return result.GetPluralizedLocalizedString(number);
                }

                return TranslatePluralizedInParent(localizationDictionary, text, number, cultureInfo, scope);
            }

            return TranslateFallback(text);
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationDictionary = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
            localizationDictionary.ListConstants().TryGetValue(text, out var result);

            if (result == null)
            {
                result = TranslateConstantInParent(localizationDictionary, text, cultureInfo, scope);
            }

            return result;
        }

        public CultureInfo DefaultCulture()
        {
            return Configuration.DefaultCulture();
        }

        public string DefaultScope()
        {
            return Localization.DefaultScope;
        }
    }
}