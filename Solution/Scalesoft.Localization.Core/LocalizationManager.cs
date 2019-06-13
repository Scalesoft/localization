using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core
{
    public class LocalizationManager : IAutoLocalizationManager
    {
        private readonly ILogger m_logger;

        private readonly LocalizationConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, ILocalizationManager> m_localizationManagers;


        public LocalizationManager(
            LocalizationConfiguration configuration,
            IDatabaseLocalizationManager databaseLocalizationManager,
            IFileLocalizationManager fileLocalizationManager,
            ILoggerFactory loggerFactory = null
        )
        {
            if (loggerFactory == null)
            {
                loggerFactory = NullLoggerFactory.Instance;
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
        private void CheckConfiguration(LocalizationConfiguration configuration)
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

        public CultureInfo[] GetSupportedCultures()
        {
            return m_configuration.SupportedCultures.ToArray();
        }

        public CultureInfo GetDefaultCulture()
        {
            return m_configuration.DefaultCulture;
        }

        public LocalizedString Translate(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope, string text)
        {
            var result = GetLocalizationManager(translationSource).Translate(cultureInfo, scope, text);

            return FallbackFilter(text, result);
        }

        [Obsolete("Use new method with params")]
        public LocalizedString TranslateFormat(
            LocTranslationSource translationSource, string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetLocalizationManager(translationSource).TranslateFormat(cultureInfo, scope, text, parameters);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateFormat(LocTranslationSource translationSource, CultureInfo cultureInfo,
            string scope, string text, params object[] parameters)
        {
            var result = GetLocalizationManager(translationSource).TranslateFormat(cultureInfo, scope, text, parameters);

            return FallbackFilter(text, result);
        }


        public LocalizedString TranslatePluralization(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope,
            string text, int number)
        {
            var result = GetLocalizationManager(translationSource).TranslatePluralization(cultureInfo, scope, text, number);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateConstant(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope, string text)
        {
            var result = GetLocalizationManager(translationSource).TranslateConstant(cultureInfo, scope, text);

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
