using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Localization.CoreLibrary.Manager.Impl
{
    internal class AutoLocalizationManager : ManagerBase, ILocalizationManager
    {
        private readonly ILocalizationManager m_fileLocalizationManager;
        private readonly ILocalizationManager m_databaseLocalizationManager;

        public AutoLocalizationManager(
            ILocalizationManager fileLocalizationManager, ILocalizationManager databaseLocalizationManager, ILocalizationConfiguration configuration,
            ILogger<AutoLocalizationManager> logger = null
        ) : base(configuration, logger)
        {
            m_fileLocalizationManager = fileLocalizationManager;
            m_databaseLocalizationManager = databaseLocalizationManager;
        }

        private ILocalizationManager GetLocalizationManager(LocLocalizationResource localizationResource)
        {
            switch (localizationResource)
            {
                case LocLocalizationResource.Database:
                    return m_databaseLocalizationManager;
                case LocLocalizationResource.File:
                    return m_fileLocalizationManager;
                default:
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(@"Requested resource type is not supported ""{0}"":""{1}""", nameof(localizationResource),
                            localizationResource);
                    }

                    throw new ArgumentOutOfRangeException(nameof(localizationResource), localizationResource, null);
            }
        }

        private ILocalizationManager GetOtherLocalizationManager(LocLocalizationResource localizationResource)
        {
            switch (localizationResource)
            {
                case LocLocalizationResource.Database:
                    return m_fileLocalizationManager;
                case LocLocalizationResource.File:
                    return m_databaseLocalizationManager;
                default:
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(@"Requested resource type is not supported ""{0}"":""{1}""", nameof(localizationResource),
                            localizationResource);
                    }

                    throw new ArgumentOutOfRangeException(nameof(localizationResource), localizationResource, null);
            }
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.Translate(text, cultureInfo, scope);
            if (result == null || result.ResourceNotFound)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource);

                return localizationManager.Translate(text, cultureInfo, scope);
            }

            return result;
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.TranslateFormat(text, parameters, cultureInfo, scope);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource);
                return localizationManager.TranslateFormat(text, parameters, cultureInfo, scope);
            }

            return result;
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.TranslatePluralization(text, number, cultureInfo, scope);
            if (result == null)
            {
                return localizationManager.TranslatePluralization(text, number, cultureInfo, scope);
            }

            return result;
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.TranslateConstant(text, cultureInfo, scope);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource);

                return localizationManager.TranslateConstant(text, cultureInfo, scope);
            }

            return result;
        }
    }
}
