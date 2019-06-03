using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Resolver;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class FileLocalizationManager : LocalizationManagerBase, IFileLocalizationManager
    {
        private readonly IFileDictionaryManager m_dictionaryManager;
        private readonly FallbackCultureResolver m_fallbackCultureResolver;

        public FileLocalizationManager(
            LocalizationConfiguration configuration,
            IFileDictionaryManager fileDictionaryManager,
            FallbackCultureResolver fallbackCultureResolver
        ) : base(configuration)
        {
            m_dictionaryManager = fileDictionaryManager;
            m_fallbackCultureResolver = fallbackCultureResolver;
        }

        public LocalizedString Translate(CultureInfo cultureInfo, string scope, string text)
        {
            while (true)
            {
                var localizationDictionary = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
                localizationDictionary.List().TryGetValue(text, out var result);

                if (result == null)
                {
                    result = TranslateInParent(localizationDictionary, text);
                }

                if (result != null)
                {
                    return result;
                }

                cultureInfo = cultureInfo == null ? null : m_fallbackCultureResolver.FallbackCulture(cultureInfo);
                if (cultureInfo != null)
                {
                    continue;
                }

                return TranslateFallback(text);
            }
        }

        private LocalizedString TranslateInParent(ILocalizationDictionary localizationDictionary, string text)
        {
            while (true)
            {
                if (localizationDictionary.ParentDictionary() == null)
                {
                    return null;
                }

                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.List().TryGetValue(text, out var result);

                if (result != null)
                {
                    return result;
                }
            }
        }

        public LocalizedString TranslateFormat(CultureInfo cultureInfo, string scope, string text, object[] parameters)
        {
            var nonParameterizedTranslation = Translate(cultureInfo, scope, text);
            var parametrizedTranslationString = string.Format(nonParameterizedTranslation.Value, parameters);

            return new LocalizedString(nonParameterizedTranslation.Name, parametrizedTranslationString);
        }

        public LocalizedString TranslatePluralization(CultureInfo cultureInfo, string scope, string text, int number)
        {
            while (true)
            {
                var localizationDictionary = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
                localizationDictionary.ListPlurals().TryGetValue(text, out var result);

                if (result == null)
                {
                    result = TranslatePluralizedInParent(localizationDictionary, text);
                }

                if (result != null)
                {
                    return result.GetPluralizedLocalizedString(number);
                }

                cultureInfo = cultureInfo == null ? null : m_fallbackCultureResolver.FallbackCulture(cultureInfo);
                if (cultureInfo != null)
                {
                    continue;
                }

                return TranslateFallback(text);
            }
        }

        private PluralizedString TranslatePluralizedInParent(ILocalizationDictionary localizationDictionary, string text)
        {
            while (true)
            {
                if (localizationDictionary.ParentDictionary() == null)
                {
                    return null;
                }

                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.ListPlurals().TryGetValue(text, out var result);

                if (result != null)
                {
                    return result;
                }
            }
        }

        public LocalizedString TranslateConstant(CultureInfo cultureInfo, string scope, string text)
        {
            while (true)
            {
                var localizationDictionary = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
                localizationDictionary.ListConstants().TryGetValue(text, out var result);

                if (result == null)
                {
                    result = TranslateConstantInParent(localizationDictionary, text);
                }

                if (result != null)
                {
                    return result;
                }

                cultureInfo = cultureInfo == null ? null : m_fallbackCultureResolver.FallbackCulture(cultureInfo);
                if (cultureInfo != null)
                {
                    continue;
                }

                return TranslateFallback(text);
            }
        }

        private LocalizedString TranslateConstantInParent(ILocalizationDictionary localizationDictionary, string text)
        {
            while (true)
            {
                if (localizationDictionary.ParentDictionary() == null)
                {
                    return null;
                }

                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.ListConstants().TryGetValue(text, out var result);

                if (result != null)
                {
                    return result;
                }
            }
        }
    }
}
