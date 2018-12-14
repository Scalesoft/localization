using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
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
        private static readonly object m_initLock = new object();

        private const string CultureJPath = "culture";
        private const string ScopeJPath = "scope";
        public const string JsonExtension = "json";
        private const string PluralJPath = "plural";

        private const string NotLoadedMsg = "Dictionary is not loaded.";
        private const string NotLoadedPluralizedMsg = "Pluralized dictionary is not loaded.";

        private JObject m_jsonDictionary;
        private JObject m_jsonPluralizedDictionary;

        private volatile ConcurrentDictionary<string, LocalizedString> m_dictionary;
        private volatile ConcurrentDictionary<string, PluralizedString> m_pluralizedDictionary;
        private volatile ConcurrentDictionary<string, LocalizedString> m_constantDictionaries;

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

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                m_jsonDictionary = LoadDictionaryJObject(fileStream, filePath);
            }


            var cultureString = (string) m_jsonDictionary[CultureJPath];
            m_cultureInfo = new CultureInfo(cultureString);

            m_scope = (string) m_jsonDictionary[ScopeJPath];

            TryLoadPluralized(filePath);

            return this;
        }

        public ILocalizationDictionary Load(Stream resourceStream)
        {
            if (IsLoaded())
            {
                Logger.LogWarning("Dictionary is already loaded.");
                return this;
            }

            m_jsonDictionary = LoadDictionaryJObject(resourceStream);

            var cultureString = (string) m_jsonDictionary[CultureJPath];
            m_cultureInfo = new CultureInfo(cultureString);

            m_scope = (string) m_jsonDictionary[ScopeJPath];

            return this;
        }

        private void TryLoadPluralized(string filePath)
        {
            var filePathWithoutExtension = Path.ChangeExtension(filePath, "");
            var newFilePath = string.Concat(filePathWithoutExtension, PluralJPath, ".", JsonExtension);

            if (!File.Exists(newFilePath))
            {
                return;
            }

            using (var fileStream = new FileStream(newFilePath, FileMode.Open, FileAccess.Read))
            {
                m_jsonPluralizedDictionary = LoadDictionaryJObject(fileStream, newFilePath);
            }

            var cultureString = (string) m_jsonPluralizedDictionary[CultureJPath];
            if (!m_cultureInfo.Equals(new CultureInfo(cultureString)))
            {
                var message = string.Format(
                    @"Culture in pluralized version of dictionary ""{0}"" does not match expected value. Expected value is ""{1}""",
                    filePath, m_cultureInfo.Name);
                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(message);
                }

                throw new DictionaryLoadException(message);
            }
        }

        private JObject LoadDictionaryJObject(Stream resourceStream, string fileName = null)
        {
            JObject dictionary;
            using (var stringReader = new StreamReader(resourceStream, Encoding.UTF8, true))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                try
                {
                    dictionary = (JObject) JToken.ReadFrom(jsonReader);
                }
                catch (JsonReaderException e)
                {
                    var message = $@"Resource file ""{fileName ?? "(stream)"}"" is not well-formatted. See library documentation.";
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

        public IDictionary<string, LocalizedString> List()
        {
            if (m_dictionary != null)
            {
                return m_dictionary;
            }

            lock (m_initLock)
            {
                if (m_dictionary != null)
                {
                    return m_dictionary;
                }

                var dictionary = InitDictionary();
                m_dictionary = dictionary;
                return dictionary;
            }
        }

        private ConcurrentDictionary<string, LocalizedString> InitDictionary()
        {
            var dictionary = new ConcurrentDictionary<string, LocalizedString>();
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }

                return dictionary;
            }


            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken("dictionary");
            if (keyValueObjects == null)
            {
                return dictionary;
            }


            var keyValueEnumerator = keyValueObjects.GetEnumerator();
            while (keyValueEnumerator.MoveNext())
            {
                var keyValuePair = keyValueEnumerator.Current;
                var ls = new LocalizedString(keyValuePair.Key, keyValuePair.Value.ToString());
                dictionary.TryAdd(ls.Name, ls);
            }

            keyValueEnumerator.Dispose();

            return dictionary;
        }

        public IDictionary<string, PluralizedString> ListPlurals()
        {
            if (m_pluralizedDictionary != null)
            {
                return m_pluralizedDictionary;
            }

            lock (m_initLock)
            {
                if (m_pluralizedDictionary != null)
                {
                    return m_pluralizedDictionary;
                }

                var pluralizedDictionary = InitPluralizedDictionary();
                m_pluralizedDictionary = pluralizedDictionary;
                return pluralizedDictionary;
            }
        }

        private ConcurrentDictionary<string, PluralizedString> InitPluralizedDictionary()
        {
            var pluralizedDictionary = new ConcurrentDictionary<string, PluralizedString>();
            if (!IsPluralizationLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedPluralizedMsg);
                }

                return pluralizedDictionary;
            }

            var keyValueObjects = (JObject) m_jsonPluralizedDictionary.SelectToken("dictionary");
            if (keyValueObjects == null)
            {
                return pluralizedDictionary;
            }

            var keyValueEnumerator = keyValueObjects.GetEnumerator();
            while (keyValueEnumerator.MoveNext())
            {
                var keyValuePair = keyValueEnumerator.Current;
                var key = keyValuePair.Key;
                var value = keyValuePair.Value.First;

                var defaultValue = ((JProperty) keyValuePair.Value.First).Name;
                var defaultLocalizedString = new LocalizedString(key, defaultValue);
                var pluralizedString = new PluralizedString(defaultLocalizedString);
                var pluralizationTriples = value.First;

                var childrenTriplesEnumerator = pluralizationTriples.Children().GetEnumerator();

                while (childrenTriplesEnumerator.MoveNext())
                {
                    var childrenTripleJToken = childrenTriplesEnumerator.Current;
                    var leftInterval = childrenTripleJToken[0];
                    var rightInterval = childrenTripleJToken[1];
                    var stringValue = childrenTripleJToken[2];

                    int leftIntervalInteger;
                    if (leftInterval?.Value<string>() == null)
                    {
                        leftIntervalInteger = int.MinValue;
                    }
                    else
                    {
                        var xParsed = int.TryParse(leftInterval.ToString(), out leftIntervalInteger);
                        if (!xParsed)
                        {
                            var errorMessage = string.Format(
                                @"The x value ""{0}"" in pluralization dictionary: ""{1}"" culture: ""{2}""",
                                leftInterval, m_scope, m_cultureInfo.Name);
                            if (Logger.IsErrorEnabled())
                            {
                                Logger.LogError(errorMessage);
                            }

                            throw new DictionaryFormatException(errorMessage);
                        }
                    }

                    int rightIntervalInteger;
                    if (rightInterval?.Value<string>() == null)
                    {
                        rightIntervalInteger = int.MaxValue;
                    }
                    else
                    {
                        var yParsed = int.TryParse(rightInterval.ToString(), out rightIntervalInteger);
                        if (!yParsed)
                        {
                            var errorMessage = string.Format(
                                @"The y value ""{0}"" in pluralization dictionary: ""{1}"" culture: ""{2}""",
                                rightInterval.ToString(), m_scope, m_cultureInfo.Name);
                            if (Logger.IsErrorEnabled())
                            {
                                Logger.LogError(errorMessage);
                            }

                            throw new DictionaryFormatException(errorMessage);
                        }
                    }

                    var stringValueString = (string) stringValue;
                    pluralizedString.Add(new PluralizationInterval(leftIntervalInteger, rightIntervalInteger),
                        new LocalizedString(key, stringValueString));
                }

                childrenTriplesEnumerator.Dispose();
                pluralizedDictionary.TryAdd(key, pluralizedString);
            }

            keyValueEnumerator.Dispose();

            return pluralizedDictionary;
        }

        public IDictionary<string, LocalizedString> ListConstants()
        {
            if (m_constantDictionaries != null)
            {
                return m_constantDictionaries;
            }

            lock (m_initLock)
            {
                if (m_constantDictionaries != null)
                {
                    return m_constantDictionaries;
                }

                var constantDictionaries = InitConstantDictionaries();
                m_constantDictionaries = constantDictionaries;
                return constantDictionaries;
            }
        }

        private ConcurrentDictionary<string, LocalizedString> InitConstantDictionaries()
        {
            var constantsDictionary = new ConcurrentDictionary<string, LocalizedString>();
            if (!IsLoaded())
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(NotLoadedMsg);
                }

                return constantsDictionary;
            }

            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken("constants");
            if (keyValueObjects == null)
            {
                return constantsDictionary;
            }


            var keyValueEnumerator = keyValueObjects.GetEnumerator();

            while (keyValueEnumerator.MoveNext())
            {
                var keyValuePair = keyValueEnumerator.Current;
                var ls = new LocalizedString(keyValuePair.Key, keyValuePair.Value.ToString());
                constantsDictionary.TryAdd(ls.Name, ls);
            }

            keyValueEnumerator.Dispose();

            return constantsDictionary;
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
            var result = false;
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

            var comparer = (ILocalizationDictionary) obj;


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
            var hashCode = CultureInfo().GetHashCode() ^ Scope().GetHashCode();
            return hashCode;
        }
    }
}
