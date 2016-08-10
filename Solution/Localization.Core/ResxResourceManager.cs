using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Localization.Core
{
    public class ResxResourceManager : ILocalizationResourceManager
    {
        private readonly ResourceManager m_resourceManager;

        public ResxResourceManager(LocalizationConfiguration configuration)
        {
            m_resourceManager = new ResourceManager(configuration.ResourceBaseName, configuration.ResourceAssembly);
        }

        public string GetString(string textKey, CultureInfo cultureInfo)
        {
            return m_resourceManager.GetString(textKey, cultureInfo);
        }

        public IDictionary<string, string> GetDictionary(CultureInfo cultureInfo)
        {
            var resourceSet = m_resourceManager.GetResourceSet(cultureInfo, true, true);
            var dictionary = resourceSet.Cast<DictionaryEntry>().ToDictionary(resource => resource.Key as string, resource => resource.Value as string);
            return dictionary;
        }
    }
}