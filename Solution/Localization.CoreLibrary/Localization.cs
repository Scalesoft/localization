using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Resolver;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary
{
    public class Localization : IAutoLocalizationManager, IAutoDictionaryManager
    {
        private readonly ILogger m_logger;

        public const string DefaultScope = "global";

        private readonly ILocalizationConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, ILocalizationManager> m_localizationManagers
            = new Dictionary<LocTranslationSource, ILocalizationManager>();

        private readonly Dictionary<LocTranslationSource, IDictionaryManager> m_dictionaryManagers
            = new Dictionary<LocTranslationSource, IDictionaryManager>();

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

            m_instance = new Lazy<Localization>(() => new Localization(
                configuration,
                databaseLocalizationManager,
                databaseDictionaryManager,
                dictionaryManager,
                fileLocalizationManager,
                loggerFactory
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
            ILocalizationConfiguration configuration,
            IDatabaseLocalizationManager databaseLocalizationManager,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IFileDictionaryManager fileDictionaryManager,
            IFileLocalizationManager fileLocalizationManager,
            ILoggerFactory loggerFactory = null
        )
        {
            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            m_logger = loggerFactory.CreateLogger<Localization>();

            CheckConfiguration(configuration);
            m_configuration = configuration;

            m_localizationManagers.Add(LocTranslationSource.Database, databaseLocalizationManager);
            m_dictionaryManagers.Add(LocTranslationSource.Database, databaseDictionaryManager);

            m_localizationManagers.Add(LocTranslationSource.File, fileLocalizationManager);
            m_dictionaryManagers.Add(LocTranslationSource.File, fileDictionaryManager);

            ILocalizationManager autoLocalizationManager = new AutoLocalizationManager(
                m_localizationManagers[LocTranslationSource.File],
                databaseLocalizationManager,
                configuration
            );

            IDictionaryManager autoDictionaryManager = new AutoDictionaryManager(
                m_dictionaryManagers[LocTranslationSource.File],
                databaseDictionaryManager,
                configuration
            );

            m_localizationManagers.Add(LocTranslationSource.Auto, autoLocalizationManager);
            m_dictionaryManagers.Add(LocTranslationSource.Auto, autoDictionaryManager);
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

        /// <summary>
        /// Adds single specified dictionary from file.
        /// </summary>
        /// <param name="dictionaryFactory">Implementation of IDictionaryFactory</param>
        /// <param name="filePath">path to file</param>
        public void AddSingleDictionary(IDictionaryFactory dictionaryFactory, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                AddSingleDictionary(dictionaryFactory, fileStream);
            }
        }

        /// <summary>
        /// Adds single specified dictionary from stream.
        /// </summary>
        /// <param name="dictionaryFactory">Implementation of IDictionaryFactory</param>
        /// <param name="resourceStream">stream</param>
        public void AddSingleDictionary(IDictionaryFactory dictionaryFactory, Stream resourceStream)
        {
            var dictionaryManager = (FileDictionaryManager) m_dictionaryManagers[LocTranslationSource.File];

            dictionaryManager.AddDictionaryToHierarchyTrees(
                dictionaryFactory.CreateDictionary(resourceStream)
            );
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

        public IDictionary<string, LocalizedString> GetDictionary(
            LocTranslationSource translationSource, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetDictionaryManager(translationSource).GetDictionary(cultureInfo, scope);

            return result;
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(
            LocTranslationSource translationSource, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetDictionaryManager(translationSource).GetPluralizedDictionary(cultureInfo, scope);

            return result;
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(
            LocTranslationSource translationSource, CultureInfo cultureInfo = null, string scope = null
        )
        {
            var result = GetDictionaryManager(translationSource).GetConstantsDictionary(cultureInfo, scope);

            return result;
        }

        private ILocalizationManager GetLocalizationManager(LocTranslationSource translationSource)
        {
            return m_localizationManagers[translationSource];
        }

        private IDictionaryManager GetDictionaryManager(LocTranslationSource translationSource)
        {
            return m_dictionaryManagers[translationSource];
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
