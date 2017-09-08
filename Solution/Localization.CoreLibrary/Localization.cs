﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        public const string DefaultScope = "global";

        private static Lazy<Localization> m_instance;
        private static IConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, ILocalizationManager> m_localizationManagers
            = new Dictionary<LocTranslationSource, ILocalizationManager>();

        private readonly Dictionary<LocTranslationSource, IDictionaryManager> m_dictionaryManagers
            = new Dictionary<LocTranslationSource, IDictionaryManager>();

        //private IDictionaryManager m_dictionaryManager;

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
        public static IAutoLocalizationManager Translator => Instance();

        public static ILocalizationManager FileTranslator =>
            m_instance.Value.GetLocalizationManager(LocTranslationSource.File);

        public static ILocalizationManager DatabaseTranslator =>
            m_instance.Value.GetLocalizationManager(LocTranslationSource.Database);

        /// <summary>
        /// Returns Dictionary.
        /// </summary>
        public static IAutoDictionaryManager Dictionary => Instance();

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
        /// <exception cref="LocalizationLibraryException">Thrown if libary is already initialized.</exception>
        public static void Init(IConfiguration configuration,
           IDatabaseServiceFactory databaseServiceFactory = null,
            IDictionaryFactory dictionaryFactory = null, 
            ILoggerFactory loggerFactory = null)
        {
            ILocalizationManager databaseLocalizationManager;
            IDictionaryManager databaseDictionaryManager;

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

            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            if (databaseServiceFactory == null)
            {
                databaseLocalizationManager = new NullDatabaseLocalization();
            }
            else
            {
                IDatabaseTranslateService dbTranslateService = databaseServiceFactory.CreateTranslateService(configuration, loggerFactory);


                databaseLocalizationManager = new DatabaseLocalizationManager(configuration, dbTranslateService);   
               
            }

            databaseDictionaryManager = new DatabaseDictionaryManager(configuration, databaseServiceFactory.CreateDictionaryService(configuration, loggerFactory));

            //m_instance = new Localization(configuration, loggerFactory, dictionaryFactory, databaseLocalization);

            m_instance = new Lazy<Localization>(() => new Localization(configuration, loggerFactory, dictionaryFactory, databaseLocalizationManager, databaseDictionaryManager));
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

            FileDictionaryManager dictionaryManager
                 = (FileDictionaryManager) Instance().m_dictionaryManagers[LocTranslationSource.File];

            dictionaryManager.Dictionaries.Add(localizationDictionary.Load(filePath));

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
        /// <exception cref="LocalizationLibraryException">Thrown if libary is already initialized.</exception>
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
        /// Private constructor.
        /// </summary>
        /// <param name="configuration">Library configuration instance.</param>
        /// <param name="dictionaryFactory">Dictionary instnace.</param>
        /// <param name="loggerFactory">Logger factory instance.</param>
        /// <param name="databaseLocalizationManager"></param>
        /// <param name="databaseDictionaryManager"></param>
        private Localization(IConfiguration configuration, 
            ILoggerFactory loggerFactory, 
            IDictionaryFactory dictionaryFactory,
            ILocalizationManager databaseLocalizationManager,
            IDictionaryManager databaseDictionaryManager)
        {
            AttachLogger(loggerFactory);           

            m_localizationManagers.Add(LocTranslationSource.Database, databaseLocalizationManager);        
            m_dictionaryManagers.Add(LocTranslationSource.Database, databaseDictionaryManager);

            InitDictionaryManager(configuration, dictionaryFactory);
            InitLocalizationManager(configuration);
            m_configuration = configuration;

            ILocalizationManager autoLocalizationManager 
                = new AutoLocalizationManager(m_localizationManagers[LocTranslationSource.File], databaseLocalizationManager, configuration);

            IDictionaryManager autoDictionaryManager = new AutoDictionaryManager(m_dictionaryManagers[LocTranslationSource.File],
                databaseDictionaryManager, configuration);


            m_localizationManagers.Add(LocTranslationSource.Auto, autoLocalizationManager);
            m_dictionaryManagers.Add(LocTranslationSource.Auto, autoDictionaryManager);
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
                throw new LocalizationLibraryException("You must initialize the Dictionary manager before FileLocalization manager");
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

        public LocalizedString Translate(LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).Translate(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateFormat(LocTranslationSource translationSource, string text, string[] parameters,
            CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).TranslateFormat(text, parameters, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

  
        public LocalizedString TranslatePluralization(LocTranslationSource translationSource, string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).TranslatePluralization(text, number, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        public LocalizedString TranslateConstant(LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LocalizedString result = GetLocalizationManager(translationSource).TranslateConstant(text, cultureInfo, scope);

            return FallbackFilter(text, result);
        }

        CultureInfo IAutoLocalizationManager.DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }

        public Dictionary<string, LocalizedString> GetDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null)
        {
            Dictionary<string, LocalizedString> result =
                GetDictonaryManager(translationSource).GetDictionary(cultureInfo, scope);

            return result;
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null)
        {
            Dictionary<string, PluralizedString> result =
                GetDictonaryManager(translationSource).GetPluralizedDictionary(cultureInfo, scope);

            return result;
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(LocTranslationSource translationSource,
            CultureInfo cultureInfo = null, string scope = null)
        {
            Dictionary<string, LocalizedString> result =
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