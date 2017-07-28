using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DictionaryManager : IDictionaryManager
    {
        private static readonly ILogger Logger  = LogProvider.GetCurrentClassLogger();

        private IConfiguration m_configuration;

        public DictionaryManager(IConfiguration configuration)
        {
            m_configuration = configuration;
            
            //1] Load config (folder hierarchy, db credentials)
            //2] Check dictionary hierarchy (folders and files)
            //      If something doesnt exist, create them and LOG it.
            //3] Lazyload dictionaries?
        }

        public HashSet<LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                return GetGlobalDictionary(cultureInfo);
            }

            return GetScopedDictionary(cultureInfo, scope);           
        }

        public HashSet<LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                //GetGlobalPartDictionary(cultureInfo)
            }
            else
            {
                //GetScopedPartDictionary(cultureInfo)
            }

            throw new System.NotImplementedException();
        }

        private HashSet<LocalizedString> GetGlobalDictionary(CultureInfo cultureInfo)
        {
            if (IsCultureSupported(cultureInfo))
            {
                //return global dictionary in requested culture

                throw new System.NotImplementedException();
            }

            return GetGlobalDictionaryWithDefaultCulture();
        }

        private HashSet<LocalizedString> GetGlobalDictionaryWithDefaultCulture()
        {
            //return global dictionary in default culture

            throw new System.NotImplementedException();
        }

        private HashSet<LocalizedString> GetScopedDictionary(CultureInfo cultureInfo, string scope)
        {
            if (IsCultureSupported(cultureInfo))
            {
                //return scoped dictionary in requested culture (if scope exists)
            }

            //return scoped dictionary in default culture

            throw new System.NotImplementedException();
        }


        //TODO: Check cultureInfo = null behavior
        private bool IsCultureSupported(CultureInfo cultureInfo)
        {
            if (m_configuration.SupportedCultures().Contains(cultureInfo) || m_configuration.DefaultCulture().Equals(cultureInfo))
            {
                return true;
            }

            return false;
        }
    }
}