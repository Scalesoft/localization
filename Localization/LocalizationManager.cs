using System.Collections.Generic;
using System.Globalization;

namespace Localization
{
    
    public class LocalizationManager
    {
        private readonly ILocalizationRepository m_repository;
        private readonly ILocalizationResourceManager m_resourceManager;
        private readonly LocalizationConfiguration m_configuration;

        private static LocalizationManager m_instance;

        public LocalizationManager(LocalizationConfiguration configuration, ILocalizationRepository repository, ILocalizationResourceManager resourceManager)
        {
            m_configuration = configuration;
            m_repository = repository;
            m_resourceManager = resourceManager;
            SetAsStaticInstance();
        }

        public void SetAsStaticInstance()
        {
            m_instance = this;
        }

        //Instance can return null when called before SetAsStaticInstance is called
        public static LocalizationManager Instance
        {
            get
            {
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

            return cultureInfo;;
        }
    }
}