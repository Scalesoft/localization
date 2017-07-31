using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DictionaryManager : IDictionaryManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private const string JsonStructureWildcardPath = "dictionary.parts.*.*";
        public const char ScopeDelimiter = '#';

        private readonly IConfiguration m_configuration;

        private Dictionary<string, JObject> m_dictionaries;

        public DictionaryManager(IConfiguration configuration)
        {
            m_configuration = configuration;                                  
        }

        public Dictionary<string, JObject> Dictionaries
        {
            get { return m_dictionaries; }
        }

        private string MakeDictionaryKey(CultureInfo cultureInfo, string scope)
        {
            if (string.IsNullOrWhiteSpace(scope))
            {
                return cultureInfo.Name;
            }

            return string.Concat(cultureInfo.Name, ScopeDelimiter, scope);
        }

        public void LoadAndCheck()
        {
            m_dictionaries = new Dictionary<string, JObject>(); 

            //TODO: If something doesnt exist, create it and LOG IT using logger.

            //Start TestInit
            using (StreamReader reader = File.OpenText(@"local\slovniky\slovniky-cs.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                m_dictionaries.Add(MakeDictionaryKey(new CultureInfo("cs"), "slovniky"), o);
            }

            using (StreamReader reader = File.OpenText(@"local\slovniky\slovniky-en.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                m_dictionaries.Add(MakeDictionaryKey(new CultureInfo("en"), "slovniky"), o);
            }

            using (StreamReader reader = File.OpenText(@"local\cs.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                m_dictionaries.Add(MakeDictionaryKey(new CultureInfo("cs"), ""), o);
            }

            using (StreamReader reader = File.OpenText(@"local\en.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                m_dictionaries.Add(MakeDictionaryKey(new CultureInfo("en"), ""), o);
            }
            //END test init
        }

        private string JsonStructurePartPath(string part)
        {
            if (string.IsNullOrWhiteSpace(part))
            {
                Logger.LogWarning(string.Format("Požadovaná část slovníku má nevalidní formu (IsNullOrWhiteSpace): \"{0}\"", part));
            }
            return string.Concat("dictionary.parts.", part, ".*");
        }

        public IEnumerable<LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                return GetGlobalDictionary(cultureInfo);
            }

            return GetScopedDictionary(cultureInfo, scope);           
        }

        public IEnumerable<LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            JObject dictionary;

            if (cultureInfo == null)
            {
                cultureInfo = m_configuration.DefaultCulture();
            }

            if (scope == null)
            {        
                m_dictionaries.TryGetValue(cultureInfo.TwoLetterISOLanguageName, out dictionary);

                return ExtractPartDictionary(dictionary, part);
            }

            //GetScopedPartDictionary(cultureInfo)
            string dictionaryKey = string.Concat(cultureInfo.TwoLetterISOLanguageName, ScopeDelimiter, scope);
            m_dictionaries.TryGetValue(dictionaryKey, out dictionary);

            return ExtractPartDictionary(dictionary, part);
        }

        private IEnumerable<LocalizedString> GetGlobalDictionary(CultureInfo cultureInfo)
        {
            if (IsCultureSupported(cultureInfo))
            {
                JObject dictionary;
                m_dictionaries.TryGetValue(cultureInfo.TwoLetterISOLanguageName, out dictionary);                                                                  
               
                //return global dictionary in requested culture
                return ExtractDictionary(dictionary);
            }

            return GetGlobalDictionaryWithDefaultCulture();
        }

        private IEnumerable<LocalizedString> GetGlobalDictionaryWithDefaultCulture()
        {          
            JObject dictionary;
            m_dictionaries.TryGetValue(m_configuration.DefaultCulture().TwoLetterISOLanguageName, out dictionary);
           
            //return global dictionary in default culture
            return ExtractDictionary(dictionary);
        }


        private IEnumerable<LocalizedString> ExtractDictionary(JObject dictionary)
        {
            return dictionary.SelectTokens(JsonStructureWildcardPath).Select(c => new LocalizedString((string)c.Path, (string)c)).ToList();
        }

        private IEnumerable<LocalizedString> ExtractPartDictionary(JObject dictionary, string part)
        {
            return dictionary.SelectTokens(JsonStructurePartPath(part)).Select(c => new LocalizedString((string)c.Path, (string)c)).ToList();
        }

        private IEnumerable<LocalizedString> GetScopedDictionary(CultureInfo cultureInfo, string scope)
        {
            JObject dictionary;
            string dictionaryKey = string.Concat(cultureInfo.TwoLetterISOLanguageName, ScopeDelimiter, scope);

            if (IsCultureSupported(cultureInfo))
            {
                //return scoped dictionary in requested culture (if scope exists)
                
                m_dictionaries.TryGetValue(dictionaryKey, out dictionary);
                return ExtractDictionary(dictionary);
            }

            m_dictionaries.TryGetValue(dictionaryKey, out dictionary);

            //return scoped dictionary in default culture
            return ExtractDictionary(dictionary);
        }


        /// <summary>
        /// Resturns true if given culture is default culture or it is in supported cultures.
        /// </summary>
        /// <param name="cultureInfo">Culture to check</param>
        /// <returns>
        /// Resturns true if given culture is default culture or it is in supported cultures.
        /// If given culture is null method returns false.
        /// </returns>
        public bool IsCultureSupported(CultureInfo cultureInfo)
        {
            if (m_configuration.DefaultCulture().Equals(cultureInfo) || m_configuration.SupportedCultures().Contains(cultureInfo))
            {
                return true;
            }

            return false;
        }
    }
}