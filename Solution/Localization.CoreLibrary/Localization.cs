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
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary
{
    public class Localization : IAutoLocalizationManager, IAutoDictionaryManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public const string DefaultScope = "global";

        private static Lazy<Localization> m_instance;
        private static IConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, ILocalizationManager> m_localizationManagers
            = new Dictionary<LocTranslationSource, ILocalizationManager>();

        private readonly Dictionary<LocTranslationSource, IDictionaryManager> m_dictionaryManagers
            = new Dictionary<LocTranslationSource, IDictionaryManager>();

        private IDatabaseDynamicTextService ddts;

        public static CultureInfo[] SupportedCultures()
        {
            return m_configuration.SupportedCultures().ToArray();
        }

        public static CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }

        CultureInfo[] IAutoLocalizationManager.SupportedCultures()
        {
            return SupportedCultures();
        }

        /// <summary>
        /// Returns Translator.
        /// </summary>
        public static IAutoLocalizationManager Translator => Instance();

        public static ILocalizationManager FileTranslator =>
            m_instance.Value.GetLocalizationManager(LocTranslationSource.File);

        public static ILocalizationManager DatabaseTranslator =>
            m_instance.Value.GetLocalizationManager(LocTranslationSource.Database);

        /// <summary>
        /// Returns Dictionary.
        /// </summary>
        public static IAutoDictionaryManager Dictionary => Instance();

        public static IDatabaseDynamicTextService DynamicText =>
            m_instance.Value.ddts;

        public static IDictionaryManager FileDictionary =>
            m_instance.Value.GetDictonaryManager(LocTranslationSource.File);

        public static IDictionaryManager DatabaseDictionary =>
            m_instance.Value.GetDictonaryManager(LocTranslationSource.Database);


        /// <summary>
        /// Returns FileLocalization library instance.
        /// </summary>
        /// <returns>FileLocalization library instance.</returns>
        /// <exception cref="LocalizationLibraryException">Thrown if FileLocalization library is not initialized.</exception>
        private static Localization Instance()
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

        private static IDictionaryFactory InitDictionaryFactory(IDictionaryFactory dictionaryFactory,
            IConfiguration configuration)
        {
            if (dictionaryFactory == null)
            {
                if (configuration.AutoLoadResources())
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

        /// <summary>
        /// Initializes FileLocalization library.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="databaseServiceFactory"></param>
        /// <param name="dictionaryFactory">Dictionary factory. 
        /// Default is <see cref="JsonDictionaryFactory"/> if AutoLoadProperties in library config is set to true. Else Default
        /// is <see cref="EmptyDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if library is already initialized.</exception>
        public static void Init(IConfiguration configuration,
            IDatabaseServiceFactory databaseServiceFactory = null,
            IDictionaryFactory dictionaryFactory = null,
            ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            if (IsInstantinated())
            {
                string libraryAlreadyInitMsg = "Localization library is already initialized.";
                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(libraryAlreadyInitMsg);
                }

                throw new LocalizationLibraryException(libraryAlreadyInitMsg);
            }

            //File dictionary factory
            dictionaryFactory = InitDictionaryFactory(dictionaryFactory, configuration);

            //Db loc manager.
            ILocalizationManager databaseLocalizationManager;
            IDatabaseDynamicTextService dbDynamicTextService = null;
            if (databaseServiceFactory == null)
            {
                databaseLocalizationManager = new NullDatabaseLocalizationManager();
            }
            else
            {
                IDatabaseTranslateService dbTranslateService =
                    databaseServiceFactory.CreateTranslateService(configuration, loggerFactory);
                dbTranslateService.CheckCulturesInDatabase();

                dbDynamicTextService =
                    databaseServiceFactory.CreateDatabaseDynamicTextService(configuration, loggerFactory);

                databaseLocalizationManager =
                    new DatabaseLocalizationManager(configuration, dbTranslateService, dbDynamicTextService);
            }

            //Db dic manager.
            IDictionaryManager databaseDictionaryManager;
            if (databaseServiceFactory == null)
            {
                databaseDictionaryManager = new NullDatabaseDictionaryManager();
            }
            else
            {
                databaseDictionaryManager = new DatabaseDictionaryManager(configuration,
                    databaseServiceFactory.CreateDictionaryService(configuration, loggerFactory));
            }


            m_instance = new Lazy<Localization>(() => new Localization(configuration, loggerFactory, dictionaryFactory,
                databaseLocalizationManager, databaseDictionaryManager, dbDynamicTextService));
        }

        /// <summary>
        /// Adds single specified dictionary from file.
        /// </summary>
        /// <param name="dictionaryFactory">Implementation of IDictionaryFactory</param>
        /// <param name="filePath">path to file</param>
        public static void AddSingleDictionary(IDictionaryFactory dictionaryFactory, string filePath)
        {
            if (!IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is not initialized.");
            }

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
        public static void AddSingleDictionary(IDictionaryFactory dictionaryFactory, Stream resourceStream)
        {
            if (!IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is not initialized.");
            }

            ILocalizationDictionary localizationDictionary = dictionaryFactory.CreateDictionary();

            FileDictionaryManager dictionaryManager
                = (FileDictionaryManager) Instance().m_dictionaryManagers[LocTranslationSource.File];

            dictionaryManager.Dictionaries.Add(localizationDictionary.Load(resourceStream));

            dictionaryManager.BuildDictionaryHierarchyTrees(dictionaryManager.Dictionaries.ToArray());
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
        public static void Init(string configFilePath,
            IDatabaseServiceFactory databaseServiceFactory = null,
            IDictionaryFactory dictionaryFactory = null,
            ILoggerFactory loggerFactory = null)
        {
            JsonConfigurationReader configurationReader = new JsonConfigurationReader(configFilePath);
            IConfiguration configuration = configurationReader.ReadConfiguration();
            Init(configuration, databaseServiceFactory, dictionaryFactory, loggerFactory);
        }

        /// <summary>
        /// Check if library is instantiated.
        /// </summary>
        /// <returns>True if library is instantiated.</returns>
        private static bool IsInstantinated()
        {
            return m_instance != null;
        }

        /// <summary>
        /// Checks if configuration is valid.
        /// </summary>
        /// <param name="configuration">Configuration to check.</param>
        private void CheckConfiguration(IConfiguration configuration)
        {
            if (!configuration.SupportedCultures().Contains(configuration.DefaultCulture()))
            {
                string defaultCultureErrorMsg = "Default language in configuration is not in supported languages.";
                LocalizationLibraryException localizationLibraryException =
                    new LocalizationLibraryException(defaultCultureErrorMsg);
                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(defaultCultureErrorMsg, localizationLibraryException);
                }

                throw localizationLibraryException;
            }
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="configuration">Library configuration instance.</param>
        /// <param name="dictionaryFactory">Dictionary instnace.</param>
        /// <param name="loggerFactory">Logger factory instance.</param>
        /// <param name="databaseLocalizationManager"></param>
        /// <param name="databaseDictionaryManager"></param>
        /// <param name="databaseDynamicTextService"></param>
        private Localization(IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IDictionaryFactory dictionaryFactory,
            ILocalizationManager databaseLocalizationManager,
            IDictionaryManager databaseDictionaryManager,
            IDatabaseDynamicTextService databaseDynamicTextService)
        {
            AttachLogger(loggerFactory);

            m_localizationManagers.Add(LocTranslationSource.Database, databaseLocalizationManager);
            m_dictionaryManagers.Add(LocTranslationSource.Database, databaseDictionaryManager);

            InitDictionaryManager(configuration, dictionaryFactory);
            InitLocalizationManager(configuration);

            CheckConfiguration(configuration);
            m_configuration = configuration;

            ILocalizationManager autoLocalizationManager
                = new AutoLocalizationManager(m_localizationManagers[LocTranslationSource.File],
                    databaseLocalizationManager, configuration);

            IDictionaryManager autoDictionaryManager = new AutoDictionaryManager(
                m_dictionaryManagers[LocTranslationSource.File],
                databaseDictionaryManager, configuration);


            m_localizationManagers.Add(LocTranslationSource.Auto, autoLocalizationManager);
            m_dictionaryManagers.Add(LocTranslationSource.Auto, autoDictionaryManager);

            ddts = databaseDynamicTextService;
        }

        /// <summary>
        /// Initializes dictionary manager.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory">Specific dictionary factory.</param>
        private void InitDictionaryManager(IConfiguration configuration, IDictionaryFactory dictionaryFactory)
        {
            FileDictionaryManager dictionaryManager = new FileDictionaryManager(configuration);

            if (configuration.AutoLoadResources())
            {
                ILocalizationDictionary[] d = dictionaryManager.AutoLoadDictionaries(dictionaryFactory);
                dictionaryManager.BuildDictionaryHierarchyTrees(d);
            }


            m_dictionaryManagers[LocTranslationSource.File] = dictionaryManager;
        }

        /// <summary>
        /// Initializes localization manager.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <exception cref="LocalizationLibraryException">Throws if dictionary manager is not already loaded.</exception>
        private void InitLocalizationManager(IConfiguration configuration)
        {
            FileLocalizationManager fileLocalizationManager = new FileLocalizationManager(configuration);
            if (m_dictionaryManagers[LocTranslationSource.File] == null)
            {
                throw new LocalizationLibraryException(
                    "You must initialize the Dictionary manager before FileLocalization manager");
            }

            fileLocalizationManager.AddDictionaryManager(m_dictionaryManagers[LocTranslationSource.File]);


            m_localizationManagers.Add(LocTranslationSource.File, fileLocalizationManager);
        }

        public static void AttachLogger(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new LocalizationLibraryException("LoggerFactory cannot be null.");
            }
            else
            {
                LogProvider.AttachLoggerFactory(loggerFactory);
            }
        }

        private ILocalizationManager GetLocalizationManager(LocTranslationSource translationSource)
        {
            return m_localizationManagers[translationSource];
        }

        private IDictionaryManager GetDictonaryManager(LocTranslationSource translationSource)
        {
            return m_dictionaryManagers[translationSource];
        }

        private LocalizedString FallbackFilter(string text, LocalizedString stringToFilter)
        {
            if (stringToFilter == null)
            {
                return TranslateFallback(text, m_configuration.TranslateFallbackMode());
            }

            return stringToFilter;
        }

        public LocalizedString Translate(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).Translate(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateFormat(LocTranslationSource translationSource, string text, object[] parameters,
            CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource)
                .TranslateFormat(text, parameters, cultureInfo, scope);

            return FallbackFilter(text, result);
        }


        public LocalizedString TranslatePluralization(LocTranslationSource translationSource, string text, int number,
            CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource)
                .TranslatePluralization(text, number, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateConstant(LocTranslationSource translationSource, string text,
            CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result =
                GetLocalizationManager(translationSource).TranslateConstant(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        CultureInfo IAutoLocalizationManager.DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }

        public IDictionary<string, LocalizedString> GetDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionary<string, LocalizedString> result =
                GetDictonaryManager(translationSource).GetDictionary(cultureInfo, scope);

            return result;
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionary<string, PluralizedString> result =
                GetDictonaryManager(translationSource).GetPluralizedDictionary(cultureInfo, scope);

            return result;
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionary<string, LocalizedString> result =
                GetDictonaryManager(translationSource).GetConstantsDictionary(cultureInfo, scope);

            return result;
        }

        CultureInfo IAutoDictionaryManager.DefaultCulture()
        {
            return DefaultCulture();
        }

        private LocalizedString TranslateFallback(string text, LocTranslateFallbackMode translateFallbackMode)
        {
            switch (translateFallbackMode)
            {
                case LocTranslateFallbackMode.Key:
                    return new LocalizedString(text, text, true);
                case LocTranslateFallbackMode.Exception:
                    string errorMessage = string.Format("String with key {0} was not found.", text);
                    throw new TranslateException(errorMessage);
                case LocTranslateFallbackMode.EmptyString:
                    return new LocalizedString(text, "", true);
                default:
                    throw new LocalizationLibraryException("Unspecified fallback mode in library configuration");
            }
        }
    }
}