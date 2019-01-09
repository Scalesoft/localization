using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core
{
    public class DictionaryManager : IAutoDictionaryManager
    {
        private readonly ILogger m_logger;

        private readonly LocalizationConfiguration m_configuration;

        private readonly Dictionary<LocTranslationSource, IDictionaryManager> m_dictionaryManagers;

        public DictionaryManager(
            LocalizationConfiguration configuration,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IFileDictionaryManager fileDictionaryManager,
            ILoggerFactory loggerFactory = null
        )
        {
            if (loggerFactory == null)
            {
                loggerFactory = NullLoggerFactory.Instance;
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

        public CultureInfo[] GetSupportedCultures()
        {
            return m_configuration.SupportedCultures.ToArray();
        }

        public CultureInfo GetDefaultCulture()
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
