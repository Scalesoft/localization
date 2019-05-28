﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Util;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Scalesoft.Localization.Core.Manager.Impl
{
    internal class AutoDictionaryManager : ManagerBase, IDictionaryManager
    {
        private readonly IDictionaryManager m_fileDictionaryManager;
        private readonly IDictionaryManager m_databaseDictionaryManager;

        public AutoDictionaryManager(
            IDictionaryManager fileDictionaryManager, IDictionaryManager databaseDictionaryManager, LocalizationConfiguration configuration,
            ILogger<AutoDictionaryManager> logger = null
        ) : base(configuration, logger)
        {
            m_fileDictionaryManager = fileDictionaryManager;
            m_databaseDictionaryManager = databaseDictionaryManager;
        }

        private IDictionaryManager GetDictionaryManager(LocLocalizationResource localizationResource)
        {
            switch (localizationResource)
            {
                case LocLocalizationResource.Database:
                    return m_databaseDictionaryManager;
                case LocLocalizationResource.File:
                    return m_fileDictionaryManager;
                default:
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(@"Requested resource type is not supported ""{0}"":""{1}""", nameof(localizationResource),
                            localizationResource);
                    }

                    throw new ArgumentOutOfRangeException(nameof(localizationResource), localizationResource, null);
            }
        }

        private IDictionaryManager GetOtherDictionaryManager(LocLocalizationResource localizationResource)
        {
            switch (localizationResource)
            {
                case LocLocalizationResource.Database:
                    return m_fileDictionaryManager;
                case LocLocalizationResource.File:
                    return m_databaseDictionaryManager;
                default:
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(@"Requested resource type is not supported ""{0}"":""{1}""", nameof(localizationResource),
                            localizationResource);
                    }

                    throw new ArgumentOutOfRangeException(nameof(localizationResource), localizationResource, null);
            }
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetDictionaryManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.GetDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                localizationManager = GetOtherDictionaryManager(m_configuration.FirstAutoTranslateResource);
                try
                {
                    result = localizationManager.GetDictionary(cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    if (m_logger != null && m_logger.IsInformationEnabled())
                    {
                        m_logger.LogInformation(@"Requested dictionary with culture ""{0}"" and scope ""{1}""", cultureInfo?.Name, scope,
                            e);
                    }

                    return null;
                }
            }

            return result;
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetDictionaryManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.GetPluralizedDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                localizationManager = GetOtherDictionaryManager(m_configuration.FirstAutoTranslateResource);
                try
                {
                    result = localizationManager.GetPluralizedDictionary(cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    if (m_logger != null && m_logger.IsInformationEnabled())
                    {
                        m_logger.LogInformation(@"Requested pluralization dictionary with culture ""{0}"" and scope ""{1}""",
                            cultureInfo?.Name, scope, e);
                    }

                    return null;
                }
            }

            return result;
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            var localizationManager = GetDictionaryManager(m_configuration.FirstAutoTranslateResource);

            var result = localizationManager.GetConstantsDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                localizationManager = GetOtherDictionaryManager(m_configuration.FirstAutoTranslateResource);
                try
                {
                    result = localizationManager.GetConstantsDictionary(cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    if (m_logger != null && m_logger.IsInformationEnabled())
                    {
                        m_logger.LogInformation(@"Requested constants dictionary with culture ""{0}"" and scope ""{1}""", cultureInfo?.Name,
                            scope, e);
                    }

                    return null;
                }
            }

            return result;
        }
    }
}
