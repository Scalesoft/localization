using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.IoC;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Localization.CoreLibrary
{
    [Obsolete("CoreLibrary.Localization is obsolete, use CoreLibrary.DictionaryManager or CoreLibrary.LocalizationManager instead.")]
    public class Localization : IAutoLocalizationManager, IAutoDictionaryManager
    {
        private readonly IAutoDictionaryManager m_dictionaryManager;
        private readonly IAutoLocalizationManager m_localizationManager;

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
        /// <param name="databaseConfiguration"></param>
        /// <param name="dictionaryFactory">Dictionary factory.
        /// Default is <see cref="JsonDictionaryFactory"/> if AutoLoadProperties in library config is set to true. Else Default
        /// is <see cref="EmptyDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if library is already initialized.</exception>
        public static void Init(
            LocalizationConfiguration configuration,
            IDatabaseConfiguration databaseConfiguration = null,
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

            m_instance = new Lazy<Localization>(() => new Localization(
                configuration,
                null,
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
            IDictionaryFactory dictionaryFactory, LocalizationConfiguration configuration
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
            LocalizationConfiguration configuration,
            IDatabaseConfiguration databaseConfiguration,
            ILoggerFactory loggerFactory
        )
        {
            var services = new ServiceCollection();
            services.AddLocalizationCore();
            services.AddSingleton(configuration);

            // Logger
            services.AddSingleton(loggerFactory ?? NullLoggerFactory.Instance);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            // Database
            if (databaseConfiguration != null)
            {
                databaseConfiguration.RegisterToIoc(services);
            }
            else
            {
                services.TryAddSingleton<IDatabaseLocalizationManager, NullDatabaseLocalizationManager>();
                services.TryAddSingleton<IDatabaseDictionaryManager, NullDatabaseDictionaryManager>();
            }
            
            // Init from IoC
            var container = services.BuildServiceProvider();

            m_localizationManager = container.GetRequiredService<IAutoLocalizationManager>();
            m_dictionaryManager = container.GetRequiredService<IAutoDictionaryManager>();
        }

        /// <summary>
        /// Initializes FileLocalization library.
        /// </summary>
        /// <param name="configFilePath">Path to configuration file.</param>
        /// <param name="databaseConfiguration"></param>
        /// <param name="dictionaryFactory">DictionaryFactory.
        /// Default is <see cref="JsonDictionaryFactory"/></param>
        /// <param name="loggerFactory">Logger factory.
        /// Default is <see cref="NullLoggerFactory"/></param>
        /// <exception cref="LocalizationLibraryException">Thrown if library is already initialized.</exception>
        public static void Init(
            string configFilePath,
            IDatabaseConfiguration databaseConfiguration = null,
            IDictionaryFactory dictionaryFactory = null,
            ILoggerFactory loggerFactory = null
        )
        {
            var configurationReader = new JsonConfigurationReader(configFilePath, loggerFactory.CreateLogger<JsonConfigurationReader>());
            var configuration = configurationReader.ReadConfiguration();

            Init(configuration, databaseConfiguration, dictionaryFactory, loggerFactory);
        }

        public CultureInfo[] SupportedCultures()
        {
            return m_localizationManager.SupportedCultures();
        }

       public CultureInfo DefaultCulture()
        {
            return m_localizationManager.DefaultCulture();
        }

        public LocalizedString Translate(
            LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.Translate(translationSource, text, cultureInfo, scope);
        }

        public LocalizedString TranslateFormat(
            LocTranslationSource translationSource, string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.TranslateFormat(translationSource, text, parameters, cultureInfo, scope);
        }

        public LocalizedString TranslatePluralization(
            LocTranslationSource translationSource, string text, int number, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.TranslatePluralization(translationSource, text,number, cultureInfo, scope);
        }

        public LocalizedString TranslateConstant(
            LocTranslationSource translationSource, string text, CultureInfo cultureInfo = null, string scope = null
        )
        {
            return m_localizationManager.TranslateConstant(translationSource, text, cultureInfo, scope);
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
}
