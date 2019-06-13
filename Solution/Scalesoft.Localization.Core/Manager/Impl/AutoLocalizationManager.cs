﻿using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Util;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Scalesoft.Localization.Core.Manager.Impl
{
    internal class AutoLocalizationManager : ManagerBase, ILocalizationManager
    {
        private readonly ILocalizationManager m_fileLocalizationManager;
        private readonly ILocalizationManager m_databaseLocalizationManager;

        public AutoLocalizationManager(
            ILocalizationManager fileLocalizationManager, ILocalizationManager databaseLocalizationManager, LocalizationConfiguration configuration,
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

        public LocalizedString Translate(CultureInfo cultureInfo, string scope, string text)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.Translate(cultureInfo, scope, text);
            if (result == null || result.ResourceNotFound)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource);

                return localizationManager.Translate(cultureInfo, scope, text);
            }

            return result;
        }

        public LocalizedString TranslateFormat(CultureInfo cultureInfo, string scope, string text, object[] parameters)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.TranslateFormat(cultureInfo, scope, text, parameters);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource);
                return localizationManager.TranslateFormat(cultureInfo, scope, text, parameters);
            }

            return result;
        }

        public LocalizedString TranslatePluralization(CultureInfo cultureInfo, string scope, string text, int number)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.TranslatePluralization(cultureInfo, scope, text, number);
            if (result == null)
            {
                return localizationManager.TranslatePluralization(cultureInfo, scope, text, number);
            }

            return result;
        }

        public LocalizedString TranslateConstant(CultureInfo cultureInfo, string scope, string text)
        {
            var localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.TranslateConstant(cultureInfo, scope, text);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource);

                return localizationManager.TranslateConstant(cultureInfo, scope, text);
            }

            return result;
        }
    }
}
