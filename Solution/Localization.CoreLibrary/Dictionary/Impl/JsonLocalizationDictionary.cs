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

        private const string DictionaryKey = "dictionary";
        private const string ConstantKey = "constants";

        private readonly CultureInfo m_cultureInfo;
        private readonly string m_scope;

        private readonly JObject m_jsonDictionary;
        private readonly JObject m_jsonPluralizedDictionary;

        private volatile ConcurrentDictionary<string, LocalizedString> m_dictionary;
        private volatile ConcurrentDictionary<string, PluralizedString> m_pluralizedDictionary;
        private volatile ConcurrentDictionary<string, LocalizedString> m_constantDictionaries;

        private ILocalizationDictionary m_parentDictionary;

        public IList<ILocalizationDictionary> ChildDictionaries { get; }

        public bool IsRoot => m_parentDictionary == null;

        public JsonLocalizationDictionary(Stream resourceStream)
        {
            ChildDictionaries = new List<ILocalizationDictionary>();

            m_jsonDictionary = LoadDictionaryJObject(resourceStream);

            var cultureString = (string) m_jsonDictionary[CultureJPath];
            m_cultureInfo = new CultureInfo(cultureString);

            m_scope = (string) m_jsonDictionary[ScopeJPath];
        }

        public JsonLocalizationDictionary(Stream resourceStream, string filePath) : this(resourceStream)
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
            return m_cultureInfo;
        }

        public string Scope()
        {
            return m_scope;
        }

        public string Extension()
        {
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
                InitDictionary();

                return m_dictionary;
            }
        }

        private void InitDictionary()
        {
            if (m_dictionary != null)
            {
                return;
            }

            var dictionary = new ConcurrentDictionary<string, LocalizedString>();

            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken(DictionaryKey);
            if (keyValueObjects == null)
            {
                m_dictionary = dictionary;

                return;
            }

            var keyValueEnumerator = keyValueObjects.GetEnumerator();
            while (keyValueEnumerator.MoveNext())
            {
                var keyValuePair = keyValueEnumerator.Current;
                var ls = new LocalizedString(keyValuePair.Key, keyValuePair.Value.ToString());
                dictionary.TryAdd(ls.Name, ls);
            }

            keyValueEnumerator.Dispose();

            m_dictionary = dictionary;
        }

        public IDictionary<string, PluralizedString> ListPlurals()
        {
            if (m_pluralizedDictionary != null)
            {
                return m_pluralizedDictionary;
            }

            lock (m_initLock)
            {
                InitPluralizedDictionary();

                return m_pluralizedDictionary;
            }
        }

        private void InitPluralizedDictionary()
        {
            if (m_pluralizedDictionary != null)
            {
                return;
            }

            var pluralizedDictionary = new ConcurrentDictionary<string, PluralizedString>();

            var keyValueObjects = (JObject) m_jsonPluralizedDictionary.SelectToken(DictionaryKey);
            if (keyValueObjects == null)
            {
                m_pluralizedDictionary = pluralizedDictionary;
                return;
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

            m_pluralizedDictionary = pluralizedDictionary;
        }

        public IDictionary<string, LocalizedString> ListConstants()
        {
            if (m_constantDictionaries != null)
            {
                return m_constantDictionaries;
            }

            lock (m_initLock)
            {
                InitConstantDictionaries();

                return m_constantDictionaries;
            }
        }

        private void InitConstantDictionaries()
        {
            if (m_constantDictionaries != null)
            {
                return;
            }

            var constantsDictionary = new ConcurrentDictionary<string, LocalizedString>();

            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken(ConstantKey);
            if (keyValueObjects == null)
            {
                m_constantDictionaries = constantsDictionary;
                return;
            }


            var keyValueEnumerator = keyValueObjects.GetEnumerator();

            while (keyValueEnumerator.MoveNext())
            {
                var keyValuePair = keyValueEnumerator.Current;
                var ls = new LocalizedString(keyValuePair.Key, keyValuePair.Value.ToString());
                constantsDictionary.TryAdd(ls.Name, ls);
            }

            keyValueEnumerator.Dispose();

            m_constantDictionaries = constantsDictionary;
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
            parentDictionary.SetChildDictionary(this);

            return true;
        }

        public void SetChildDictionary(ILocalizationDictionary childDictionary)
        {
            ChildDictionaries.Add(childDictionary);
        }

        public bool IsLeaf()
        {
            return ChildDictionaries.Count == 0;
        }

        private bool Equals(JsonLocalizationDictionary other)
        {
            return m_cultureInfo.Equals(other.m_cultureInfo) && string.Equals(m_scope, other.m_scope);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((JsonLocalizationDictionary) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (m_cultureInfo.GetHashCode() * 397) ^ m_scope.GetHashCode();
            }
        }
    }
}
