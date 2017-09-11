using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
namespace Localization.CoreLibrary.Manager.Impl
{
    internal class AutoDictionaryManager : IDictionaryManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly IDictionaryManager m_fileDictionaryManager;
        private readonly IDictionaryManager m_databaseDictionaryManager;

        private readonly IConfiguration m_configuration;

        public AutoDictionaryManager(IDictionaryManager fileDictionaryManager, IDictionaryManager databaseDictionaryManager, IConfiguration configuration)
        {
            m_fileDictionaryManager = fileDictionaryManager;
            m_databaseDictionaryManager = databaseDictionaryManager;
            m_configuration = configuration;
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
                    if (Logger.IsErrorEnabled())
                    {
                        Logger.LogError(@"Requested resource type is not supported ""{0}"":""{1}""", nameof(localizationResource),localizationResource);
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
                    if (Logger.IsErrorEnabled())
                    {
                        Logger.LogError(@"Requested resource type is not supported ""{0}"":""{1}""", nameof(localizationResource), localizationResource);
                    }

                    throw new ArgumentOutOfRangeException(nameof(localizationResource), localizationResource, null);
            }
        }

        public Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionaryManager localizationManager = GetDictionaryManager(m_configuration.FirstAutoTranslateResource());

            Dictionary<string, LocalizedString> result = localizationManager.GetDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                localizationManager = GetOtherDictionaryManager(m_configuration.FirstAutoTranslateResource());
                try
                {
                    result = localizationManager.GetDictionary(cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    if (Logger.IsInformationEnabled())
                    {
                        Logger.LogInformation(@"Requested dictionary with culture ""{0}"" and scope ""{1}""", cultureInfo?.Name, scope, e);
                    }

                    return null;
                }
            }

            return result;
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionaryManager localizationManager = GetDictionaryManager(m_configuration.FirstAutoTranslateResource());

            Dictionary<string, PluralizedString> result =
                localizationManager.GetPluralizedDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                localizationManager = GetOtherDictionaryManager(m_configuration.FirstAutoTranslateResource());
                try
                {
                    result = localizationManager.GetPluralizedDictionary(cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    if (Logger.IsInformationEnabled())
                    {
                        Logger.LogInformation(@"Requested pluralization dictionary with culture ""{0}"" and scope ""{1}""", cultureInfo?.Name, scope, e);
                    }

                    return null;
                }
            }

            return result;
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            IDictionaryManager localizationManager = GetDictionaryManager(m_configuration.FirstAutoTranslateResource());

            Dictionary<string, LocalizedString> result =
                localizationManager.GetConstantsDictionary(cultureInfo, scope);
            if (result == null || result.Count == 0)
            {
                localizationManager = GetOtherDictionaryManager(m_configuration.FirstAutoTranslateResource());
                try
                {
                    result = localizationManager.GetConstantsDictionary(cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    if (Logger.IsInformationEnabled())
                    {
                        Logger.LogInformation(@"Requested contants dictionary with culture ""{0}"" and scope ""{1}""", cultureInfo?.Name, scope, e);
                    }

                    return null;
                }
            }

            return result;
        }

        public CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }

        public string DefaultScope()
        {
            return Localization.DefaultScope;
        }
    }
}