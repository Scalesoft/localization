using System.Collections.Generic;
using System.Globalization;
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
    public class Localization : ILocalizationManager, IDictionaryManager
    {
        private static volatile Localization m_instance;
        private static readonly object SyncObj = new object();

        private ILocalizationManager m_localizationManager;
        private IDictionaryManager m_dictionaryManager;

        /// <summary>
        /// Returns Translator.
        /// </summary>
        public static ILocalizationManager Translator
        {
            get
            {
                return Instance();
            }
        }

        /// <summary>
        /// Returns Dictionary.
        /// </summary>
        public static IDictionaryManager Dictionary
        {
            get { return Instance(); }
        }

        /// <summary>
        /// Returns Localization library instance.
        /// </summary>
        /// <returns>Localization library instance.</returns>
        /// <exception cref="LocalizationLibraryException">Thrown if Localization library is not initialized.</exception>
        private static Localization Instance()
        {
            if (!IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is not initialized.");
            }
            return m_instance;
        }

        /// <summary>
        /// Deinits Localization library instance.
        /// </summary>
        /// <exception cref="LocalizationLibraryException">Thrown if Localization library is not initialized.</exception>
        public static void LibDeinit()
        {
            if (!IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is not initialized.");
            }          
            m_instance = null;            
        }

        /// <summary>
        /// Initializes Localization library.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory">Dictionary factory. 
        /// Default is <see cref="JsonDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if libary is already initialized.</exception>
        public static void LibInit(IConfiguration configuration, 
            IDictionaryFactory dictionaryFactory = null, 
            ILoggerFactory loggerFactory = null)
        {
            if (IsInstantinated())
            {
                throw new LocalizationLibraryException("Localization library is already initialized.");
            }
            if (dictionaryFactory == null)
            {
                dictionaryFactory = new JsonDictionaryFactory(); 
            }
            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            m_instance = new Localization(configuration, loggerFactory, dictionaryFactory);
        }

        /// <summary>
        /// Initializes Localization library.
        /// </summary>
        /// <param name="configFilePath">Path to configuration file.</param>
        /// <param name="dictionaryFactory">DictionaryFactory.
        /// Default is <see cref="JsonDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if libary is already initialized.</exception>
        public static void LibInit(string configFilePath, 
            IDictionaryFactory dictionaryFactory = null, 
            ILoggerFactory loggerFactory = null)
        {
            JsonConfigurationReader configurationReader = new JsonConfigurationReader(configFilePath);
            IConfiguration configuration = configurationReader.ReadConfiguration();
            LibInit(configuration, dictionaryFactory, loggerFactory);
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
        private Localization(IConfiguration configuration, ILoggerFactory loggerFactory, IDictionaryFactory dictionaryFactory)
        {
            AttachLogger(loggerFactory);        

            InitDictionaryManager(configuration, dictionaryFactory);
            InitLocalizationManager(configuration);
        }

        /// <summary>
        /// Initializes dictionary manager.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory">Specific dictionary factory.</param>
        private void InitDictionaryManager(IConfiguration configuration, IDictionaryFactory dictionaryFactory)
        {
            DictionaryManager dictionaryManager = new DictionaryManager(configuration);
            ILocalizationDictionary[] d = dictionaryManager.AutoLoadDictionaries(dictionaryFactory);
            dictionaryManager.BuildDictionaryHierarchyTrees(d);

            m_dictionaryManager = dictionaryManager;
        }

        /// <summary>
        /// Initializes localization manager.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <exception cref="LocalizationLibraryException">Throws if dictionary manager is not already loaded.</exception>
        private void InitLocalizationManager(IConfiguration configuration)
        {
            LocalizationManager localizationManager = new LocalizationManager(configuration);
            if (m_dictionaryManager == null)
            {
                throw new LocalizationLibraryException("You must initialize the Dictionary manager before Localization manager");
            }

            localizationManager.AddDictionaryManager(m_dictionaryManager);

            m_localizationManager = localizationManager;
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

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_localizationManager.Translate(text, cultureInfo, scope);
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_localizationManager.TranslateFormat(text, parameters, cultureInfo, scope);
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_localizationManager.TranslatePluralization(text, number, cultureInfo, scope);
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_localizationManager.TranslateConstant(text, cultureInfo, scope);
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
    }
}