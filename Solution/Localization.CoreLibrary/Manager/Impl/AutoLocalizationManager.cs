using System;
using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class AutoLocalizationManager : ILocalizationManager
    {
        private readonly ILocalizationManager m_fileLocalizationManager;
        private readonly ILocalizationManager m_databaseLocalizationManager;

        private readonly IConfiguration m_configuration;

        public AutoLocalizationManager(ILocalizationManager fileLocalizationManager, ILocalizationManager databaseLocalizationManager, IConfiguration configuration)
        {
            m_fileLocalizationManager = fileLocalizationManager;
            m_databaseLocalizationManager = databaseLocalizationManager;
            m_configuration = configuration;
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
                    throw new ArgumentOutOfRangeException(nameof(localizationResource), localizationResource, null);
            }
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationManager localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource());

            LocalizedString result = localizationManager.Translate(text, cultureInfo, scope);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource());
                try
                {
                    result = localizationManager.Translate(text, cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    return null;
                }              
            }

            return result;
        }

        public LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationManager localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource());

            LocalizedString result = localizationManager.TranslateFormat(text, parameters, cultureInfo, scope);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource());               
                try
                {
                    result = localizationManager.TranslateFormat(text, parameters, cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    return null;
                }
            }

            return result;
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationManager localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource());

            LocalizedString result = localizationManager.TranslatePluralization(text, number, cultureInfo, scope);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource());               
                try
                {
                    result = localizationManager.TranslatePluralization(text, number, cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
                    return null;
                }
            }

            return result;
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            ILocalizationManager localizationManager = GetLocalizationManager(m_configuration.FirstAutoTranslateResource());

            LocalizedString result = localizationManager.TranslateConstant(text, cultureInfo, scope);
            if (result == null)
            {
                localizationManager = GetOtherLocalizationManager(m_configuration.FirstAutoTranslateResource());              
                try
                {
                    result = localizationManager.TranslateConstant(text, cultureInfo, scope);
                }
                catch (DatabaseLocalizationManagerException e)
                {
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