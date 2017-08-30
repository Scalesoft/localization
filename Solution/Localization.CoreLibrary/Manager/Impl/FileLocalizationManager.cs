using System;
using System.Globalization;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class FileLocalizationManager : ILocalizationManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private const string GlobalScope = "global";

        private readonly IConfiguration m_configuration;
        private DictionaryManager m_dictionaryManager;

        public FileLocalizationManager(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public void AddDictionaryManager(IDictionaryManager dictionaryManager)
        {
            m_dictionaryManager = (DictionaryManager)dictionaryManager;
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationDictionary ls = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
            LocalizedString result;
            ls.List().TryGetValue(text, out result);
            if (result == null)
            {
                result = TranslateInParent(ls, text, cultureInfo, scope);
            }
            return result;
        }

        private LocalizedString TranslateInParent(ILocalizationDictionary localizationDictionary, string text, CultureInfo cultureInfo = null, string scope = null)
        {
            if (localizationDictionary.ParentDictionary() != null)
            {
                LocalizedString result;
                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.List().TryGetValue(text, out result);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return TranslateInParent(localizationDictionary, text, cultureInfo, scope);
                }
            }
            else
            {
                return TranslateFallback(text);
            }
        }

        private LocalizedString TranslateConstantInParent(ILocalizationDictionary localizationDictionary, string text, CultureInfo cultureInfo = null, string scope = null)
        {
            if (localizationDictionary.ParentDictionary() != null)
            {
                LocalizedString result;
                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.ListConstants().TryGetValue(text, out result);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return TranslateConstantInParent(localizationDictionary, text, cultureInfo, scope);
                }
            }
            else
            {
                return TranslateFallback(text);
            }
        }

        private LocalizedString TranslateFallback(string text)
        {
            switch (m_configuration.TranslateFallbackMode())
            {
                case TranslateFallbackMode.Null:
                    return null;
                case TranslateFallbackMode.Key:
                    return new LocalizedString(text,text, true);
                case TranslateFallbackMode.Exception:
                    throw new TranslateException("string with text " + text + " not found");
                case TranslateFallbackMode.EmptyString:
                    return new LocalizedString(text,"");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString unparametrizedTranslation = Translate(text, cultureInfo, scope);
            string parametrizedTranslationStrng = string.Format(unparametrizedTranslation.Value, parameters);

            return new LocalizedString(unparametrizedTranslation.Name, parametrizedTranslationStrng);
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationDictionary ls = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
            PluralizedString resultPluralizedString;
            ls.ListPlurals().TryGetValue(text, out resultPluralizedString);
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
                PluralizedString result;
                localizationDictionary = localizationDictionary.ParentDictionary();

                localizationDictionary.ListPlurals().TryGetValue(text, out result);

                if (result != null)
                {
                    return result.GetPluralizedLocalizedString(number);
                }
                else
                {
                    return TranslatePluralizedInParent(localizationDictionary, text, number, cultureInfo, scope);
                }
            }
            else
            {
                return TranslateFallback(text);
            }
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationDictionary localizationDictionary = m_dictionaryManager.GetLocalizationDictionary(cultureInfo, scope);
            LocalizedString result;
            localizationDictionary.ListConstants().TryGetValue(text, out result);
            if (result == null)
            {
                result = TranslateConstantInParent(localizationDictionary, text, cultureInfo, scope);
            }
            return result;
        }

        public CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }
    }
}