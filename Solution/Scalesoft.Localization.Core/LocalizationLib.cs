﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.IoC;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Util;
using Scalesoft.Localization.Core.Util.Impl;

namespace Scalesoft.Localization.Core
{
    public class LocalizationLib : IAutoLocalizationManager, IAutoDictionaryManager
    {
        private readonly IAutoDictionaryManager m_dictionaryManager;
        private readonly IAutoLocalizationManager m_localizationManager;

        /// <summary>
        /// Initializes Localization library.
        /// </summary>
        /// <param name="configuration">Library configuration</param>
        /// <param name="databaseConfiguration">Configure database storage, e.g. NHibernateDatabaseConfiguration</param>
        /// <param name="loggerFactory">Logger factory for enabling logging</param>
        public LocalizationLib(
            LocalizationConfiguration configuration,
            IDatabaseConfiguration databaseConfiguration = null,
            ILoggerFactory loggerFactory = null
        )
        {
            var services = new ServiceCollection();
            services.AddLocalizationCore(configuration, databaseConfiguration);
            
            // Logger
            services.AddSingleton(loggerFactory ?? NullLoggerFactory.Instance);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            // Init from IoC
            var container = services.BuildServiceProvider();

            m_localizationManager = container.GetRequiredService<IAutoLocalizationManager>();
            m_dictionaryManager = container.GetRequiredService<IAutoDictionaryManager>();
        }

        /// <summary>
        /// Initializes Localization library.
        /// </summary>
        /// <param name="configFilePath">Path to configuration file.</param>
        /// <param name="databaseConfiguration">Configure database storage, e.g. NHibernateDatabaseConfiguration</param>
        /// <param name="loggerFactory">Logger factory for enabling logging</param>
        public LocalizationLib(
            string configFilePath,
            IDatabaseConfiguration databaseConfiguration = null,
            ILoggerFactory loggerFactory = null
        ) : this(LoadConfigurationFromFile(configFilePath, loggerFactory), databaseConfiguration, loggerFactory)
        {
            // Empty, only call different constructor
        }

        public static LocalizationConfiguration LoadConfigurationFromFile(string configFilePath, ILoggerFactory loggerFactory = null)
        {
            loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            var logger = loggerFactory.CreateLogger<JsonConfigurationReader>();
            var configurationReader = new JsonConfigurationReader(configFilePath, logger);
            var configuration = configurationReader.ReadConfiguration();
            return configuration;
        }

        public IAutoDictionaryManager DictionaryManager => m_dictionaryManager;

        public IAutoLocalizationManager LocalizationManager => m_localizationManager;

        public CultureInfo[] GetSupportedCultures()
        {
            return m_localizationManager.GetSupportedCultures();
        }

        public CultureInfo GetDefaultCulture()
        {
            return m_localizationManager.GetDefaultCulture();
        }

        public LocalizedString Translate(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope, string text)
        {
            return m_localizationManager.Translate(translationSource, cultureInfo, scope, text);
        }

        public LocalizedString TranslateFormat(LocTranslationSource translationSource, CultureInfo cultureInfo,
            string scope, string text, params object[] parameters)
        {
            return m_localizationManager.TranslateFormat(translationSource, cultureInfo, scope, text, parameters);
        }

        public LocalizedString TranslatePluralization(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope,
            string text, int number)
        {
            return m_localizationManager.TranslatePluralization(translationSource, cultureInfo, scope, text, number);
        }

        public LocalizedString TranslateConstant(LocTranslationSource translationSource, CultureInfo cultureInfo, string scope, string text)
        {
            return m_localizationManager.TranslateConstant(translationSource, cultureInfo, scope, text);
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

        public void AddSingleDictionary(IDictionaryFactory dictionaryFactory, string filePath)
        {
            m_dictionaryManager.AddSingleDictionary(dictionaryFactory, filePath);
        }

        public void AddSingleDictionary(IDictionaryFactory dictionaryFactory, Stream resourceStream)
        {
            m_dictionaryManager.AddSingleDictionary(dictionaryFactory, resourceStream);
        }
    }

    [Obsolete("CoreLibrary.Localization is obsolete, use CoreLibrary.LocalizationLib instead.")]
    public class Localization
    {
    }
}
