using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Resolver;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary
{
    [Obsolete("CoreLibrary.Localization is obsolete, use CoreLibrary.DictionaryManager or CoreLibrary.LocalizationManager inestead.")]
    public class Localization : IAutoLocalizationManager, IAutoDictionaryManager
    {
        private readonly DictionaryManager m_dictionaryManager;
        private readonly LocalizationManager m_localizationManager;

        /// <summary>
        /// [Deprecated use IoC]
        /// </summary>
        private static Lazy<Localization> m_instance;

        /// <summary>
        /// [Deprecated use IoC] Returns FileLocalization library instance.
        /// </summary>
        /// <returns>FileLocalization library instance.</returns>
        /// <exception cref="LocalizationLibraryException">Thrown if FileLocalization library is not initialized.</exception>
        public static Localization Instance()
        {
            if (!IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is not initialized.");
            }

            return m_instance.Value;
        }

        /// <summary>
        /// Deinits FileLocalization library instance.
        /// </summary>
        /// <exception cref="LocalizationLibraryException">Thrown if FileLocalization library is not initialized.</exception>
        public static void LibDeinit()
        {
            m_instance = null;
        }

        /// <summary>
        /// [Deprecated use IoC] Initializes FileLocalization library.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="databaseServiceFactory"></param>
        /// <param name="dictionaryFactory">Dictionary factory.
        /// Default is <see cref="JsonDictionaryFactory"/> if AutoLoadProperties in library config is set to true. Else Default
        /// is <see cref="EmptyDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if library is already initialized.</exception>
        public static void Init(
            ILocalizationConfiguration configuration,
            IDatabaseServiceFactory databaseServiceFactory = null,
            IDictionaryFactory dictionaryFactory = null,
            ILoggerFactory loggerFactory = null
        )
        {
            if (IsInstantinated())
            {
                const string libraryAlreadyInitMsg = "Localization library is already initialized.";
                var logger = loggerFactory?.CreateLogger<Localization>();

                if (logger != null && logger.IsErrorEnabled())
                {
                    logger.LogError(libraryAlreadyInitMsg);
                }

                throw new LocalizationLibraryException(libraryAlreadyInitMsg);
            }

            IDatabaseLocalizationManager databaseLocalizationManager;
            IDatabaseDictionaryManager databaseDictionaryManager;

            //Db loc manager.
            if (databaseServiceFactory == null)
            {
                databaseLocalizationManager = new NullDatabaseLocalizationManager(
                    loggerFactory?.CreateLogger<NullDatabaseDictionaryManager>()
                );
            }
            else
            {
                var dbTranslateService = databaseServiceFactory.CreateTranslateService(configuration, loggerFactory);
                dbTranslateService.CheckCulturesInDatabase();

                var dbDynamicTextService = databaseServiceFactory.CreateDatabaseDynamicTextService(configuration, loggerFactory);

                databaseLocalizationManager = new DatabaseLocalizationManager(configuration, dbTranslateService, dbDynamicTextService);
            }

            //Db dic manager.
            if (databaseServiceFactory == null)
            {
                databaseDictionaryManager = new NullDatabaseDictionaryManager(
                    loggerFactory?.CreateLogger<NullDatabaseDictionaryManager>()
                );
            }
            else
            {
                databaseDictionaryManager = new DatabaseDictionaryManager(
                    configuration,
                    databaseServiceFactory.CreateDictionaryService(configuration, loggerFactory)
                );
            }

            //File dictionary factory
            dictionaryFactory = InitDictionaryFactory(dictionaryFactory, configuration);
            var dictionaryManager = new FileDictionaryManager(configuration, dictionaryFactory);

            var fallbackCultureResolver = new FallbackCultureResolver(configuration);

            var fileLocalizationManager = new FileLocalizationManager(
                configuration,
                dictionaryManager,
                fallbackCultureResolver
            );

            var autoLocalizationManager = new LocalizationManager(
                configuration,
                databaseLocalizationManager,
                fileLocalizationManager,
                loggerFactory
            );

            var autoDictionaryManager = new DictionaryManager(
                configuration,
                databaseDictionaryManager,
                dictionaryManager,
                loggerFactory
            );

            m_instance = new Lazy<Localization>(() => new Localization(
                autoLocalizationManager,
                autoDictionaryManager
            ));
        }

        /// <summary>
        /// Check if library is instantiated.
        /// </summary>
        /// <returns>True if library is instantiated.</returns>
        private static bool IsInstantinated()
        {
            return m_instance != null;
        }

        private static IDictionaryFactory InitDictionaryFactory(
            IDictionaryFactory dictionaryFactory, ILocalizationConfiguration configuration
        )
        {
            if (dictionaryFactory == null)
            {
                if (configuration.AutoLoadResources)
                {
                    dictionaryFactory = new JsonDictionaryFactory();
                }
                else
                {
                    dictionaryFactory = new EmptyDictionaryFactory();
                }
            }

            return dictionaryFactory;
        }

        public Localization(
            LocalizationManager localizationManager,
            DictionaryManager dictionaryManager
        )
        {
            m_localizationManager = localizationManager;
            m_dictionaryManager = dictionaryManager;
        }

        /// <summary>
        /// Initializes FileLocalization library.
        /// </summary>
        /// <param name="configFilePath">Path to configuration file.</param>
        /// <param name="databaseServiceFactory"></param>
        /// <param name="dictionaryFactory">DictionaryFactory.
        /// Default is <see cref="JsonDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if library is already initialized.</exception>
        public static void Init(
            string configFilePath,
            IDatabaseServiceFactory databaseServiceFactory = null,
            IDictionaryFactory dictionaryFactory = null,
            ILoggerFactory loggerFactory = null
        )
        {
            var configurationReader = new JsonConfigurationReader(configFilePath, loggerFactory.CreateLogger<JsonConfigurationReader>());
            var configuration = configurationReader.ReadConfiguration();

            Init(configuration, databaseServiceFactory, dictionaryFactory, loggerFactory);
        }

        public CultureInfo[] SupportedCultures()
        {
            return m_localizationManager.SupportedCultures();
        }

        public CultureInfo DefaultCulture()
        {
            return m_localizationManager.DefaultCulture();
        }

        public LocalizedString Translate(
            LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.Translate(translationSource, text, cultureInfo, scope);
        }

        public LocalizedString TranslateFormat(
            LocTranslationSource translationSource, string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.TranslateFormat(translationSource, text, parameters, cultureInfo, scope);
        }

        public LocalizedString TranslatePluralization(
            LocTranslationSource translationSource, string text, int number, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.TranslatePluralization(translationSource, text,number, cultureInfo, scope);
        }

        public LocalizedString TranslateConstant(
            LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.TranslateConstant(translationSource, text, cultureInfo, scope);
        }

        public IDictionary<string, LocalizedString> GetDictionary(
            LocTranslationSource translationSource, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_dictionaryManager.GetDictionary(translationSource, cultureInfo, scope);
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(
            LocTranslationSource translationSource, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_dictionaryManager.GetPluralizedDictionary(translationSource, cultureInfo, scope);
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(
            LocTranslationSource translationSource, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_dictionaryManager.GetConstantsDictionary(translationSource, cultureInfo, scope);
        }
    }
}
