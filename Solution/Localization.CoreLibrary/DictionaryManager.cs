using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary
{
    public class DictionaryManager : IAutoDictionaryManager
    {
        private readonly ILogger m_logger;

        private readonly ILocalizationConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, IDictionaryManager> m_dictionaryManagers;

        public DictionaryManager(
            ILocalizationConfiguration configuration,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IFileDictionaryManager fileDictionaryManager,
            ILoggerFactory loggerFactory = null
        )
        {
            if (loggerFactory == null)
            {
                loggerFactory = new NullLoggerFactory();
            }

            m_logger = loggerFactory.CreateLogger<DictionaryManager>();
            m_dictionaryManagers = new Dictionary<LocTranslationSource, IDictionaryManager>();

            CheckConfiguration(configuration);
            m_configuration = configuration;

            m_dictionaryManagers.Add(LocTranslationSource.Database, databaseDictionaryManager);

            m_dictionaryManagers.Add(LocTranslationSource.File, fileDictionaryManager);

            IDictionaryManager autoDictionaryManager = new AutoDictionaryManager(
                m_dictionaryManagers[LocTranslationSource.File],
                databaseDictionaryManager,
                configuration
            );

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

        public CultureInfo[] SupportedCultures()
        {
            return m_configuration.SupportedCultures.ToArray();
        }

        public CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture;
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

        private IDictionaryManager GetDictionaryManager(LocTranslationSource translationSource)
        {
            return m_dictionaryManagers[translationSource];
        }
    }
}