using System.Collections.Generic;
using System.Globalization;

namespace Localization.Core
{
    
    public sealed class LocalizationManager
    {
        private readonly ILocalizationRepository m_repository;
        private readonly ILocalizationResourceManager m_resourceManager;
        private readonly LocalizationConfiguration m_configuration;

        private volatile static LocalizationManager m_instance;
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


        public string Translate(string text, CultureInfo cultureInfo = null)
        {
            cultureInfo = ResolveCultureInfo(cultureInfo);
            return m_resourceManager.GetString(text, cultureInfo);
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