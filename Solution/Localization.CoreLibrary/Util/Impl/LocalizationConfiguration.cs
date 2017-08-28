﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Manager;

namespace Localization.CoreLibrary.Util.Impl
{
    public sealed class LocalizationConfiguration : IConfiguration
    {
        private readonly Configuration m_configuration;
        private IList<CultureInfo> m_supportedCultures;


        public LocalizationConfiguration()
        {
            m_configuration = new Configuration();
        }

        public LocalizationConfiguration(LocalizationConfiguration.Configuration configuration)
        {
            m_configuration = configuration;

            m_supportedCultures = new List<CultureInfo>();
            foreach (var cultureName in configuration.SupportedCultures)
            {
                m_supportedCultures.Add(new CultureInfo(cultureName));
            }
        }

        public string BasePath()
        {
            return m_configuration.BasePath;
        }

        public IList<CultureInfo> SupportedCultures()
        {
            m_supportedCultures = new List<CultureInfo>();
            foreach (var cultureName in m_configuration.SupportedCultures)
            {
                m_supportedCultures.Add(new CultureInfo(cultureName));
            }

            return m_supportedCultures;
        }

        public CultureInfo DefaultCulture()
        {
            return new CultureInfo(m_configuration.DefaultCulture);
        }

        public TranslateFallbackMode TranslateFallbackMode()
        {
            TranslateFallbackMode translateFallbackModeEnum = Manager.TranslateFallbackMode.Null;
            bool parseSuccess = Enum.TryParse(m_configuration.TranslationFallbackMode, out translateFallbackModeEnum);
            if (!parseSuccess || translateFallbackModeEnum.Equals(Manager.TranslateFallbackMode.Null))
            {
                string message = string.Format(@"Cannot parse TranslateFallbackMode value ""{0}""", m_configuration.TranslationFallbackMode);

                throw new LibraryConfigurationException(message);
            }

            return translateFallbackModeEnum;
        }

        public bool AutoLoadResources()
        {
            return m_configuration.AutoLoadResources;
        }

        public EnLocalizationResource FirstAutoTranslateResource()
        {
            EnLocalizationResource firstAutoTranslateResourceEnum;
            bool parseSuccess = Enum.TryParse(m_configuration.FirstAutoTranslateResource, out firstAutoTranslateResourceEnum);
            if (!parseSuccess)
            {
                string message = string.Format(@"Cannot parse FirstAutoTranslateResource value ""{0}""", m_configuration.FirstAutoTranslateResource);

                throw new LibraryConfigurationException(message);
            }

            return firstAutoTranslateResourceEnum;
        }

        public string DbSource()
        {
            return m_configuration.DbSource;
        }

        public string DbUser()
        {
            return m_configuration.DbUser;
        }

        public string DbPassword()
        {
            return m_configuration.DbPassword;
        }

        public LocalizationConfiguration SetBasePath(string basePath)
        {
            m_configuration.BasePath = basePath;
            return this;
        }

        public LocalizationConfiguration SetSupportedCultures(IList<string> supportedCultures)
        {
            m_configuration.SupportedCultures = supportedCultures;

            return this;
        }

        public LocalizationConfiguration SetSupportedCultures(string[] supportedCultures)
        {
            m_configuration.SupportedCultures = new List<string>(supportedCultures);

            return this;
        }

        public LocalizationConfiguration SetDefaultCulture(CultureInfo defaultCulture)
        {
            m_configuration.DefaultCulture = defaultCulture.ToString();

            return this;
        }

        public LocalizationConfiguration SetDbSource(string dbSource)
        {
            m_configuration.DbSource = dbSource;

            return this;
        }

        public LocalizationConfiguration SetDbUser(string dbUser)
        {
            m_configuration.DbUser = dbUser;

            return this;
        }

        public LocalizationConfiguration SetDbPass(string dbPass)
        {
            m_configuration.DbPassword = dbPass;

            return this;
        }

        public LocalizationConfiguration SetTranslationFallbackMode(TranslateFallbackMode translationFallbackMode)
        {
            m_configuration.TranslationFallbackMode = translationFallbackMode.ToString();

            return this;
        }

        public LocalizationConfiguration SetAutoLoadResources(bool autoLoadResources)
        {
            m_configuration.AutoLoadResources = autoLoadResources;

            return this;
        }

        public LocalizationConfiguration SetFirstAutoTranslateResource(EnLocalizationResource localizationResource)
        {
            m_configuration.FirstAutoTranslateResource = localizationResource.ToString();

            return this;
        }


        public class Configuration
        {
            public string BasePath { get; set; }
            public IList<string> SupportedCultures { get; set; }
            public string DefaultCulture { get; set; }

            public string TranslationFallbackMode { get; set; }
            public bool AutoLoadResources { get; set; }
            public string FirstAutoTranslateResource { get; set; }

            public string DbSource { get; set; }
            public string DbUser { get; set; }
            public string DbPassword { get; set; }
        }
    }
}