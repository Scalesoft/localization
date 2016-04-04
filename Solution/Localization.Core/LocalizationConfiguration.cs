using System.Reflection;

namespace Localization.Core
{
    public class LocalizationConfiguration
    {
        private readonly string m_resourceBaseName;
        private readonly Assembly m_resourceAssembly;
        private readonly string m_defaultCulture;

        public LocalizationConfiguration(string resourceBaseName, string resourceAssembly, string defaultCulture)
        {
            m_resourceAssembly = Assembly.Load(resourceAssembly);
            m_defaultCulture = defaultCulture;
            m_resourceBaseName = resourceBaseName;
        }

        public string ResourceBaseName => m_resourceBaseName;

        public Assembly ResourceAssembly => m_resourceAssembly;

        public string DefaultCulture => m_defaultCulture;
    }
}