using System;
using System.Collections.Generic;
using System.Globalization;
using Castle.Core.Internal;

namespace Localization.Core
{
    
    public sealed class LocalizationManager
    {
        private readonly ILocalizationRepository m_repository;
        private readonly ILocalizationResourceManager m_resourceManager;
        private readonly LocalizationConfiguration m_configuration;

        private readonly string m_scopeDelimeter = "-";

        private static volatile LocalizationManager m_instance;
        private static readonly object m_lock = new object();


        private LocalizationManager()
        {
            m_configuration = Container.Current.Resolve<LocalizationConfiguration>();
            m_repository = Container.Current.Resolve<ILocalizationRepository>();
            m_resourceManager = Container.Current.Resolve<ILocalizationResourceManager>();
        }

        
        public static LocalizationManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new LocalizationManager();
                        }
                    }
                }
                return m_instance;
            }
        }

        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            m_repository.SetCurrentCultureInfo(cultureInfo);
        }

        public CultureInfo GetCultureInfo()
        {
            return m_repository.GetCurrentCultureInfo();
        }


        public string Translate(string textKey, string scope = "", string[] parameters = null, CultureInfo cultureInfo = null)
        {
            cultureInfo = ResolveCultureInfo(cultureInfo);

            var translationKey = scope.IsNullOrEmpty() ? textKey : string.Format("{0}{1}{2}", scope, m_scopeDelimeter, textKey);

            var translation =  m_resourceManager.GetString(translationKey, cultureInfo);

            return parameters == null ? translation : string.Format(translation, parameters);
        }

        public string Translate(string textKey, string[] parameters, CultureInfo cultureInfo = null)
        {
            return Translate(textKey, "", parameters, cultureInfo);
        }

        public IDictionary<string, string> GetTranslation(CultureInfo cultureInfo = null)
        {
            cultureInfo = ResolveCultureInfo(cultureInfo);
            return m_resourceManager.GetDictionary(cultureInfo);
        }


        private CultureInfo ResolveCultureInfo(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                cultureInfo = m_repository.GetCurrentCultureInfo();
            }

            if (cultureInfo == null)
            {
                cultureInfo = new CultureInfo(m_configuration.DefaultCulture);
            }

            return cultureInfo;
        }
    }
}