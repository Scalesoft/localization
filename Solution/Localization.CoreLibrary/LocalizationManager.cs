using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary
{
    public class LocalizationManager : IAutoLocalizationManager
    {
        private readonly ILogger m_logger;

        private readonly ILocalizationConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, ILocalizationManager> m_localizationManagers;


        public LocalizationManager(
            ILocalizationConfiguration configuration,
            IDatabaseLocalizationManager databaseLocalizationManager,
            IFileLocalizationManager fileLocalizationManager,
            ILoggerFactory loggerFactory = null
        )
        {
            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            m_logger = loggerFactory.CreateLogger<LocalizationManager>();
            m_localizationManagers = new Dictionary<LocTranslationSource, ILocalizationManager>();

            CheckConfiguration(configuration);
            m_configuration = configuration;

            m_localizationManagers.Add(LocTranslationSource.Database, databaseLocalizationManager);

            m_localizationManagers.Add(LocTranslationSource.File, fileLocalizationManager);

            ILocalizationManager autoLocalizationManager = new AutoLocalizationManager(
                m_localizationManagers[LocTranslationSource.File],
                databaseLocalizationManager,
                configuration
            );

            m_localizationManagers.Add(LocTranslationSource.Auto, autoLocalizationManager);
        }

        /// <summary>
        /// Checks if configuration is valid.
        /// </summary>
        /// <param name="configuration">Configuration to check.</param>
        private void CheckConfiguration(ILocalizationConfiguration configuration)
        {
            if (!configuration.SupportedCultures.Contains(configuration.DefaultCulture))
            {
                const string defaultCultureErrorMsg = "Default language in configuration is not in supported languages.";
                var localizationLibraryException = new LocalizationLibraryException(defaultCultureErrorMsg);

                if (m_logger != null && m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(defaultCultureErrorMsg, localizationLibraryException);
                }

                throw localizationLibraryException;
            }
        }

        public CultureInfo[] SupportedCultures()
        {
            return m_configuration.SupportedCultures.ToArray();
        }

        public CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture;
        }

        public LocalizedString Translate(
            LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetLocalizationManager(translationSource).Translate(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateFormat(
            LocTranslationSource translationSource, string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetLocalizationManager(translationSource).TranslateFormat(text, parameters, cultureInfo, scope);

            return FallbackFilter(text, result);
        }


        public LocalizedString TranslatePluralization(
            LocTranslationSource translationSource, string text, int number, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetLocalizationManager(translationSource).TranslatePluralization(text, number, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateConstant(
            LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetLocalizationManager(translationSource).TranslateConstant(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        private ILocalizationManager GetLocalizationManager(LocTranslationSource translationSource)
        {
            return m_localizationManagers[translationSource];
        }

        private LocalizedString FallbackFilter(string text, LocalizedString stringToFilter)
        {
            if (stringToFilter == null)
            {
                return TranslateFallback(text, m_configuration.TranslateFallbackMode);
            }

            return stringToFilter;
        }

        private LocalizedString TranslateFallback(string text, LocTranslateFallbackMode translateFallbackMode)
        {
            switch (translateFallbackMode)
            {
                case LocTranslateFallbackMode.Key:
                    return new LocalizedString(text, text, true);
                case LocTranslateFallbackMode.Exception:
                    var errorMessage = string.Format("String with key {0} was not found.", text);
                    throw new TranslateException(errorMessage);
                case LocTranslateFallbackMode.EmptyString:
                    return new LocalizedString(text, "", true);
                default:
                    throw new LocalizationLibraryException("Unspecified fallback mode in library configuration");
            }
        }
    }
}