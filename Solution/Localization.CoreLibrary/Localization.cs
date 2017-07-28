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

        private readonly ILocalizationManager m_localizationManager;
        private readonly IDictionaryManager m_dictionaryManager;

        public static ILocalizationManager Translator => m_instance;
        public static IDictionaryManager Dictionary => m_instance;

        public static void LibInit(IConfiguration configuration)
        {
            bool isInstantinated = IsInstantinated();
            if (isInstantinated)
            {
                throw new Exception("Localization library is already initialized.");
            }

            m_instance = new Localization(configuration);
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

        private Localization(IConfiguration configuration)
        {
            m_dictionaryManager = new DictionaryManager(configuration);
            m_localizationManager = new LocalizationManager(configuration);
        }

        public static void AttachLogger(ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);
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

        public HashSet<LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetDictionary(cultureInfo, scope);
        }

        public HashSet<LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            return m_dictionaryManager.GetDictionaryPart(part, cultureInfo, scope);
        }
    }
}