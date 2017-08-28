using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Impl;
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
    public class Localization : IAutoLocalizationManager, IDictionaryManager
    {
        private static volatile Localization m_instance;
        private static IConfiguration m_configuration;
        private static readonly object SyncObj = new object();

        private ILocalizationManager m_fileLocalizationManager;
        private readonly ILocalizationManager m_databaseLocalizationManager;
        private readonly ILocalizationManager m_autoLocalizationManager;
        private IDictionaryManager m_dictionaryManager;

        public static CultureInfo[] SupportedCultures()
        {
            return m_configuration.SupportedCultures().ToArray();
        }

        public static CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }

        /// <summary>
        /// Returns Translator.
        /// </summary>
        public static IAutoLocalizationManager Translator
        {
            get
            {
                return Instance();
            }
        }

        public static ILocalizationManager FileTranslator
        {
            get { return m_instance.m_fileLocalizationManager; }
        }

        public static ILocalizationManager DatabaseTranslator
        {
            get { return m_instance.m_databaseLocalizationManager; }
        }

        /// <summary>
        /// Returns Dictionary.
        /// </summary>
        public static IDictionaryManager Dictionary
        {
            get { return Instance(); }
        }

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
            return m_instance;
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
        /// Initializes FileLocalization library.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="databaseLocalization"></param>
        /// <param name="dictionaryFactory">Dictionary factory. 
        /// Default is <see cref="JsonDictionaryFactory"/> if AutoLoadProperties in library config is set to true. Else Default
        /// is <see cref="EmptyDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if libary is already initialized.</exception>
        public static void Init(IConfiguration configuration,
            ILocalizationManager databaseLocalization = null,
            IDictionaryFactory dictionaryFactory = null, 
            ILoggerFactory loggerFactory = null)
        {
            if (IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is already initialized.");
            }
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

            if (databaseLocalization == null)
            {
                databaseLocalization = new NullDatabaseLocalization();
            }

            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            // m_instance = new Localization(configuration, loggerFactory, dictionaryFactory, databaseLocalization);

            m_instance = new Lazy<Localization>(() => new Localization(configuration, loggerFactory, dictionaryFactory, databaseLocalization)).Value;
        }

        /// <summary>
        /// Adds single specified dictionary.
        /// </summary>
        /// <param name="dictionaryFactory">Implementation of IDictionaryFactory</param>
        /// <param name="filePath">path to file</param>
        public static void AddSingleDictionary(IDictionaryFactory dictionaryFactory, string filePath)
        {
            if (!IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is not initialized.");
            }

            ILocalizationDictionary localizationDictionary = dictionaryFactory.CreateDictionary();

            DictionaryManager dictionaryManager
                 = (DictionaryManager) Instance().m_dictionaryManager;

            dictionaryManager.Dictionaries.Add(localizationDictionary.Load(filePath));

            dictionaryManager.BuildDictionaryHierarchyTrees(dictionaryManager.Dictionaries.ToArray());
        }

        /// <summary>
        /// Initializes FileLocalization library.
        /// </summary>
        /// <param name="configFilePath">Path to configuration file.</param>
        /// <param name="databaseLocalization"></param>
        /// <param name="dictionaryFactory">DictionaryFactory.
        /// Default is <see cref="JsonDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if libary is already initialized.</exception>
        public static void Init(string configFilePath, 
            ILocalizationManager databaseLocalization = null,
            IDictionaryFactory dictionaryFactory = null, 
            ILoggerFactory loggerFactory = null)
        {
            JsonConfigurationReader configurationReader = new JsonConfigurationReader(configFilePath);
            IConfiguration configuration = configurationReader.ReadConfiguration();
            Init(configuration, databaseLocalization, dictionaryFactory, loggerFactory);
        }

        /// <summary>
        /// Check if library is instantiated.
        /// </summary>
        /// <returns>True if library is instantiated.</returns>
        private static bool IsInstantinated()
        {
            if (m_instance == null)
            {
                lock (SyncObj)
                {
                    if (m_instance == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="configuration">Library configuration instance.</param>
        /// <param name="dictionaryFactory">Dictionary instnace.</param>
        /// <param name="loggerFactory">Logger factory instance.</param>
        /// <param name="databaseLocalization"></param>
        private Localization(IConfiguration configuration, ILoggerFactory loggerFactory, 
            IDictionaryFactory dictionaryFactory, ILocalizationManager databaseLocalization)
        {
            AttachLogger(loggerFactory);           

            m_databaseLocalizationManager = databaseLocalization;           
            InitDictionaryManager(configuration, dictionaryFactory);
            InitLocalizationManager(configuration);
            m_configuration = configuration;

            m_autoLocalizationManager = new AutoLocalizationManager(m_fileLocalizationManager, databaseLocalization, configuration);
        }

        /// <summary>
        /// Initializes dictionary manager.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory">Specific dictionary factory.</param>
        private void InitDictionaryManager(IConfiguration configuration, IDictionaryFactory dictionaryFactory)
        {
            DictionaryManager dictionaryManager = new DictionaryManager(configuration);

            if (configuration.AutoLoadResources())
            {
                ILocalizationDictionary[] d = dictionaryManager.AutoLoadDictionaries(dictionaryFactory);
                dictionaryManager.BuildDictionaryHierarchyTrees(d);
            }
            

            m_dictionaryManager = dictionaryManager;
        }

        /// <summary>
        /// Initializes localization manager.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <exception cref="LocalizationLibraryException">Throws if dictionary manager is not already loaded.</exception>
        private void InitLocalizationManager(IConfiguration configuration)
        {
            FileLocalizationManager fileLocalizationManager = new FileLocalizationManager(configuration);
            if (m_dictionaryManager == null)
            {
                throw new LocalizationLibraryException("You must initialize the Dictionary manager before FileLocalization manager");
            }

            fileLocalizationManager.AddDictionaryManager(m_dictionaryManager);

            m_fileLocalizationManager = fileLocalizationManager;
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

        private ILocalizationManager GetLocalizationManager(EnTranslationSource translationSource)
        {
            switch (translationSource)
            {
                case EnTranslationSource.File:
                    return m_fileLocalizationManager;
                case EnTranslationSource.Database:
                    return m_databaseLocalizationManager;
                case EnTranslationSource.Auto:
                    return m_autoLocalizationManager;
                default:
                    throw new ArgumentOutOfRangeException(nameof(translationSource), translationSource, null);
            }
        }

        private LocalizedString FallbackFilter(string text, LocalizedString stringToFilter)
        {
            if (stringToFilter == null)
            {
                return TranslateFallback(text, m_configuration.TranslateFallbackMode());
            }

            return stringToFilter;
        }

        public LocalizedString Translate(EnTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).Translate(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateFormat(EnTranslationSource translationSource, string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).TranslateFormat(text, parameters, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslatePluralization(EnTranslationSource translationSource, string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).TranslatePluralization(text, number, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateConstant(EnTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).TranslateConstant(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetDictionary(cultureInfo, scope);
        }

        public Dictionary<string, LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetDictionaryPart(part, cultureInfo, scope);
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetPluralizedDictionary(cultureInfo, scope);
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetConstantsDictionary(cultureInfo, scope);
        }

        private LocalizedString TranslateFallback(string text, TranslateFallbackMode translateFallbackMode)
        {
            switch (translateFallbackMode)
            {
                case TranslateFallbackMode.Key:
                    return new LocalizedString(text, text, true);
                case TranslateFallbackMode.Exception:
                    string errorMessage = string.Format("String with key {0} was not found.", text);               
                    throw new TranslateException(errorMessage);
                case TranslateFallbackMode.EmptyString:
                    return new LocalizedString(text, "", true);
                default:
                    throw new LocalizationLibraryException("Unspecified fallback mode in library configuration");
            }
        }
    }
}