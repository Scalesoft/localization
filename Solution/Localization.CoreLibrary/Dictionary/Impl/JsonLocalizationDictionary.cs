using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
namespace Localization.CoreLibrary.Dictionary.Impl
{
    internal class JsonLocalizationDictionary : ILocalizationDictionary
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private const string CultureJPath = "culture";
        private const string ScopeJPath = "scope";
        public const string JsonExtension = "json";
        private const string PluralJPath = "plural";

        private const string NotLoadedMsg = "Dictionary is not loaded.";
        private const string NotLoadedPluralizedMsg = "Pluralized dictionary is not loaded.";

        private JObject m_jsonDictionary;
        private JObject m_jsonPluralizedDictionary;

        private Dictionary<string, LocalizedString> m_dictionary;
        private Dictionary<string, PluralizedString> m_pluralizedDictionary;
        private Dictionary<string, LocalizedString> m_constnantsDictionary;

        private ILocalizationDictionary m_parentDictionary;
        private ILocalizationDictionary m_childDictionary;   

        private CultureInfo m_cultureInfo;
        private string m_scope;

        public JsonLocalizationDictionary(string filePath)
        {          
            Load(filePath);
        }

        public JsonLocalizationDictionary()
        {
            //SHOULD BE EMPTY
        }

        public ILocalizationDictionary Load(string filePath)
        {
            if (IsLoaded())
            {
                Logger.LogWarning(string.Concat("Dictionary in: ", filePath, " is already loaded."));
                return this;
            }

            m_jsonDictionary = LoadDictionaryJObject(filePath);

            string cultureString = (string)m_jsonDictionary[CultureJPath];
            m_cultureInfo = new CultureInfo(cultureString);

            m_scope = (string)m_jsonDictionary[ScopeJPath];

            TryLoadPluralized(filePath);

            return this;
        }

        private void TryLoadPluralized(string filePath)
        {
            string filePathWithoutExtension = Path.ChangeExtension(filePath, "");
            string newFilePath = string.Concat(filePathWithoutExtension, PluralJPath, ".",JsonExtension);

            if (!File.Exists(newFilePath))
            {
                return;
            }
            m_jsonPluralizedDictionary = LoadDictionaryJObject(newFilePath);
            string cultureString = (string)m_jsonPluralizedDictionary[CultureJPath];
            if (!m_cultureInfo.Equals(new CultureInfo(cultureString)))
            {
                string message = string.Format(@"Culture in pluralized version of dictionary ""{0}"" does not match expected value.
                                                    Expected value is ""{1}""", filePath, m_cultureInfo.Name);
                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(message);
                }
                
                throw new DictionaryLoadException(message);
            }          
        }

        private JObject LoadDictionaryJObject(string filePath)
        {
            JObject dictionary;

            using (StreamReader reader = new StreamReader(new FileStream(filePath, FileMode.Open), Encoding.UTF8, true))
            {
                try
                {
                    dictionary = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                }
                catch (JsonReaderException e)
                {
                    string message = string.Format(@"Resource file ""{0}"" is not well-formated. See library documentation.",filePath);
                    Logger.LogError(message);

                    throw new LocalizationDictionaryException(string.Concat(message, "\nsrc: ", e.Message));
                }              
            }

            return dictionary;
        }


        public CultureInfo CultureInfo()
        {
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }                
            }

            return m_cultureInfo; 
        }

        public string Scope()
        {
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }
            }

            return m_scope; 
        }

        public string Extension()
        {
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }
            }

            return JsonExtension;           
        }

        public Dictionary<string, LocalizedString> List()
        {
            if (m_dictionary != null)
            {
                return m_dictionary;
            }

            m_dictionary = new Dictionary<string, LocalizedString>();
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }
                return m_dictionary;
            }
            

            JObject keyValueObjects = (JObject)m_jsonDictionary.SelectToken("dictionary");
            if (keyValueObjects == null)
            {
                return m_dictionary;
            }


            IEnumerator<KeyValuePair<string, JToken>> keyValueEnumerator = keyValueObjects.GetEnumerator();
            while (keyValueEnumerator.MoveNext())
            {
                KeyValuePair<string, JToken> keyValuePair = keyValueEnumerator.Current;
                LocalizedString ls = new LocalizedString(keyValuePair.Key, keyValuePair.Value.ToString());
                m_dictionary.Add(ls.Name, ls);
            }
            keyValueEnumerator.Dispose();

            return m_dictionary;
        }

        public Dictionary<string, PluralizedString> ListPlurals()
        {
            if (m_pluralizedDictionary != null)
            {
                return m_pluralizedDictionary;
            }

            m_pluralizedDictionary = new Dictionary<string, PluralizedString>();
            if (!IsPluralizationLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedPluralizedMsg);
                }
                return m_pluralizedDictionary;
            }

            JObject keyValueObjects = (JObject)m_jsonPluralizedDictionary.SelectToken("dictionary");
            if (keyValueObjects == null)
            {
                return m_pluralizedDictionary;
            }

            IEnumerator<KeyValuePair<string, JToken>> keyValueEnumerator = keyValueObjects.GetEnumerator();
            while (keyValueEnumerator.MoveNext())
            {
                KeyValuePair<string, JToken> keyValuePair = keyValueEnumerator.Current;
                string key = keyValuePair.Key;
                JToken value = keyValuePair.Value.First;

                string defaultValue = ((JProperty)keyValuePair.Value.First).Name;
                LocalizedString defaultLocalizedString = new LocalizedString(key, defaultValue);
                PluralizedString pluralizedString = new PluralizedString(defaultLocalizedString);
                JToken pluralizationTriples = value.First;
                JEnumerable<JToken> childrenTriples = pluralizationTriples.Children();
                IEnumerator<JToken> childrenTriplesEnumerator = childrenTriples.GetEnumerator();
                while (childrenTriplesEnumerator.MoveNext())
                {
                    JToken childrenTripleJToken = childrenTriplesEnumerator.Current;
                    JToken leftInterval = childrenTripleJToken[0];
                    JToken rigthInterval = childrenTripleJToken[1];
                    JToken stringValue = childrenTripleJToken[2];

                    Int32 leftIntervalInteger;
                    if (leftInterval == null || leftInterval.Value<string>() == null)
                    {
                        leftIntervalInteger = Int32.MinValue;
                    }
                    else
                    {
                        bool xParsed = int.TryParse(leftInterval.ToString(), out leftIntervalInteger);
                        if (!xParsed)
                        {
                            string errorMessage = string.Format(@"The x value ""{0}"" in pluralization dictionary: ""{1}"" culture: ""{2}""",
                                leftInterval.ToString(), m_scope, m_cultureInfo.Name);
                            if (Logger.IsErrorEnabled())
                            {
                                Logger.LogError(errorMessage);
                            }
                            throw new DictionaryFormatException(errorMessage);
                        }
                    }
                    Int32 rightIntervalInteger;
                    if (rigthInterval == null || rigthInterval.Value<string>() == null)
                    {
                        rightIntervalInteger = Int32.MaxValue;
                    }
                    else
                    {
                        bool yParsed = int.TryParse(rigthInterval.ToString(), out rightIntervalInteger);
                        if (!yParsed)
                        {
                            string errorMessage = string.Format(@"The y value ""{0}"" in pluralization dictionary: ""{1}"" culture: ""{2}""",
                                rigthInterval.ToString(), m_scope, m_cultureInfo.Name);
                            if (Logger.IsErrorEnabled())
                            {
                                Logger.LogError(errorMessage);
                            }
                            throw new DictionaryFormatException(errorMessage);
                        }
                    }

                    string stringValueString = (string) stringValue;
                    pluralizedString.Add(new PluralizationInterval(leftIntervalInteger, rightIntervalInteger), new LocalizedString(key, stringValueString));

                    
                }
                childrenTriplesEnumerator.Dispose();
                m_pluralizedDictionary.Add(key, pluralizedString);
                
            }
            keyValueEnumerator.Dispose();

            return m_pluralizedDictionary;

        }

        public Dictionary<string, LocalizedString> ListConstants()
        {
            if (m_constnantsDictionary != null)
            {
                return m_constnantsDictionary;
            }

            m_constnantsDictionary = new Dictionary<string, LocalizedString>();
            Dictionary<string, LocalizedString> result = new Dictionary<string, LocalizedString>();
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }
                return result;
            }

            JObject keyValueObjects = (JObject)m_jsonDictionary.SelectToken("constants");
            if (keyValueObjects == null)
            {
                return m_constnantsDictionary;
            }


            IEnumerator<KeyValuePair<string, JToken>> keyValueEnumerator = keyValueObjects.GetEnumerator();

            while (keyValueEnumerator.MoveNext())
            {
                KeyValuePair<string, JToken> keyValuePair = keyValueEnumerator.Current;
                LocalizedString ls = new LocalizedString(keyValuePair.Key, keyValuePair.Value.ToString());
                m_constnantsDictionary.Add(ls.Name, ls);
            }
            keyValueEnumerator.Dispose();

            return m_constnantsDictionary;
        }

   

        public ILocalizationDictionary ParentDictionary()
        {
            return m_parentDictionary;
        }

        public bool SetParentDictionary(ILocalizationDictionary parentDictionary)
        {
            if (parentDictionary == null)
            {
                return false;              
            }

            m_parentDictionary = parentDictionary;
            return parentDictionary.SetChildDictionary(this);
        }

        public ILocalizationDictionary ChildDictionary()
        {
            return m_childDictionary;
        }

        public bool SetChildDictionary(ILocalizationDictionary childDictionary)
        {         
            bool result = false;
            if (m_childDictionary == null)
            {
                result = true;
                m_childDictionary = childDictionary;
            }

            return result;
        }

        public bool IsLeaf()
        {
            if (m_childDictionary == null)
            {
                return true;
            }

            return false;
        }

        bool ILocalizationDictionary.IsRoot { get; set; }


        /// <summary>
        /// Returns true if json file was loaded.
        /// </summary>
        /// <returns>True if json file was loaded.</returns>
        private bool IsLoaded()
        {
            if (m_jsonDictionary == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Return true if json file containing pluralized strings was loaded.
        /// </summary>
        /// <returns>True if pluralized json file was loaded.</returns>
        private bool IsPluralizationLoaded()
        {
            if (m_jsonPluralizedDictionary == null)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ILocalizationDictionary comparer = (ILocalizationDictionary) obj;


            if (this.Scope().Equals(comparer.Scope()))
            {
                if (this.CultureInfo().Equals(comparer.CultureInfo()))
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = CultureInfo().GetHashCode() ^ Scope().GetHashCode();
            return hashCode;
        }
    }
}