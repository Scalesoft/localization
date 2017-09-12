using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Util;

namespace Localization.CoreLibrary
{
    public sealed class LocalizationConfiguration : IConfiguration
    {
        private readonly Configuration m_configuration;
        private IImmutableList<CultureInfo> m_supportedCultures;


        public LocalizationConfiguration()
        {
            m_configuration = new Configuration();
        }

        public LocalizationConfiguration(Configuration configuration)
        {
            m_configuration = configuration;

            m_supportedCultures = SupportedCultures();
        }

        public string BasePath()
        {
            return m_configuration.BasePath;
        }

        public IImmutableList<CultureInfo> SupportedCultures()
        {
            if (m_supportedCultures == null)
            {
                IList<CultureInfo> supportedCultures = new List<CultureInfo>();
                foreach (var cultureName in m_configuration.SupportedCultures)
                {
                    supportedCultures.Add(new CultureInfo(cultureName));
                }

                m_supportedCultures = supportedCultures.ToImmutableList();
            }

            return m_supportedCultures;
        }

        public CultureInfo DefaultCulture()
        {
            return new CultureInfo(m_configuration.DefaultCulture);
        }

        public LocTranslateFallbackMode TranslateFallbackMode()
        {
            LocTranslateFallbackMode translateFallbackModeEnum = LocTranslateFallbackMode.Null;
            bool parseSuccess = Enum.TryParse(m_configuration.TranslationFallbackMode, out translateFallbackModeEnum);
            if (!parseSuccess || translateFallbackModeEnum.Equals(LocTranslateFallbackMode.Null))
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

        public LocLocalizationResource FirstAutoTranslateResource()
        {
            LocLocalizationResource firstAutoTranslateResourceEnum;
            bool parseSuccess = Enum.TryParse(m_configuration.FirstAutoTranslateResource, out firstAutoTranslateResourceEnum);
            if (!parseSuccess)
            {
                string message = string.Format(@"Cannot parse FirstAutoTranslateResource value ""{0}""", m_configuration.FirstAutoTranslateResource);

                throw new LibraryConfigurationException(message);
            }

            return firstAutoTranslateResourceEnum;
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

        public LocalizationConfiguration SetTranslationFallbackMode(LocTranslateFallbackMode translationFallbackMode)
        {
            m_configuration.TranslationFallbackMode = translationFallbackMode.ToString();

            return this;
        }

        public LocalizationConfiguration SetAutoLoadResources(bool autoLoadResources)
        {
            m_configuration.AutoLoadResources = autoLoadResources;

            return this;
        }

        public LocalizationConfiguration SetFirstAutoTranslateResource(LocLocalizationResource localizationResource)
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
        }
    }
}