using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
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

        public static ILocalizationManager Translator
        {
            get
            {
                return Instance();
            }
        }

        public static IDictionaryManager Dictionary
        {
            get { return Instance(); }
        }

        private static Localization Instance()
        {
            if (m_instance == null)
            {
                throw new Exception("Localization library is not initialized.");
            }
            return m_instance;
        }

        public static void LibDeinit()
        {
            bool isInstantinated = IsInstantinated();
            if (!isInstantinated)
            {
                throw new Exception("Localization library is not initialized.");
            }

            m_instance = null;            
        }

        public static void LibInit(IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            bool isInstantinated = IsInstantinated();
            if (isInstantinated)
            {
                throw new Exception("Localization library is already initialized.");
            }

            m_instance = new Localization(configuration, loggerFactory);
        }

        public static void LibInit(string configFilePath)
        {
            JsonConfigurationReader configurationReader = new JsonConfigurationReader(configFilePath);
            IConfiguration configuration = configurationReader.ReadConfiguration();
            LibInit(configuration);
        }

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

        private Localization(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            AttachLogger(loggerFactory);        

            InitDictionaryManager(configuration);
            InitLocalizationManager(configuration);
        }

        private void InitDictionaryManager(IConfiguration configuration)
        {
            DictionaryManager dictionaryManager = new DictionaryManager(configuration);
            dictionaryManager.LoadAndCheck();

            m_dictionaryManager = dictionaryManager;
        }

        private void InitLocalizationManager(IConfiguration configuration)
        {
            LocalizationManager localizationManager = new LocalizationManager(configuration);
            if (m_dictionaryManager == null)
            {
                throw new Exception("You must initialize the Dictionary manager before Localization manager");
            }

            localizationManager.AddDictionaryManager(m_dictionaryManager);

            m_localizationManager = localizationManager;
        }

        public static void AttachLogger(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                LogProvider.AttachLoggerFactory(new NullLoggerFactory());
            }
            else
            {
                LogProvider.AttachLoggerFactory(loggerFactory);
            }
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null, string part = null)
        {
            return m_localizationManager.Translate(text, cultureInfo, scope, part);
        }

        public LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null,
            string part = null)
        {
            return m_localizationManager.TranslateFormat(text, parameters, cultureInfo, scope);
        }

        public IEnumerable<LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetDictionary(cultureInfo, scope);
        }

        public IEnumerable<LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetDictionaryPart(part, cultureInfo, scope);
        }
    }
}