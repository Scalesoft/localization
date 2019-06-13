using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Resolver;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Scalesoft.Localization.Core.Dictionary.Impl
{
    internal class JsonLocalizationDictionary : ILocalizationDictionary
    {
        private static readonly object m_initLock = new object();

        public const string JsonExtension = "json";

        private const string CultureJPath = "culture";
        private const string ScopeJPath = "scope";
        private const string ScopeAliasJPath = "scopeAlias";
        private const string PluralJPath = "plural";
        private const string DictionaryJPath = "dictionary";
        private const string ConstantJPath = "constants";
        private const string ParentScopeJPath = "parentScope";

        private readonly ILogger m_logger;
        private readonly CultureInfo m_cultureInfo;
        private readonly string m_scope;
        private readonly string m_parentScopeName;
        private readonly IList<string> m_scopeAlias;

        private readonly JObject m_jsonDictionary;
        
        private volatile ConcurrentDictionary<string, LocalizedString> m_dictionary;
        private volatile ConcurrentDictionary<string, PluralizedString> m_pluralizedDictionary;
        private volatile ConcurrentDictionary<string, LocalizedString> m_constantDictionaries;

        private ILocalizationDictionary m_parentDictionary;

        public IList<ILocalizationDictionary> ChildDictionaries { get; }

        public bool IsRoot => m_parentDictionary == null;

        public JsonLocalizationDictionary(Stream resourceStream, ILogger logger = null)
        {
            m_logger = logger;

            ChildDictionaries = new List<ILocalizationDictionary>();

            m_jsonDictionary = LoadDictionaryJObject(resourceStream);

            var cultureString = (string) m_jsonDictionary[CultureJPath];
            m_cultureInfo = new CultureInfo(cultureString);

            m_scope = (string) m_jsonDictionary[ScopeJPath];
            m_parentScopeName = (string) m_jsonDictionary[ParentScopeJPath];

            m_scopeAlias = new List<string>();
            if (m_jsonDictionary[ScopeAliasJPath] != null)
            {
                foreach (var alias in m_jsonDictionary[ScopeAliasJPath])
                {
                    m_scopeAlias.Add((string) alias);
                }
            }
        }

        public JsonLocalizationDictionary(
            Stream resourceStream, string filePath, IParentScopeResolver parentScopeResolver, ILogger logger = null
        ) : this(resourceStream, logger)
        {
            m_parentScopeName = m_parentScopeName ?? parentScopeResolver?.ResolveParentScope(filePath);
        }

        public JsonLocalizationDictionary(
            Stream resourceStream, string filePath, ILogger logger = null
        ) : this(resourceStream, filePath, new FileNameBasedParentScopeResolver(), logger)
        {
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
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(message);
                    }

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

        public IList<string> ScopeAlias()
        {
            return m_scopeAlias;
        }

        public string GetParentScopeName()
        {
            return m_parentScopeName;
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

            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken(DictionaryJPath);
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

            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken(PluralJPath);
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
                            var errorMessage =
                                $@"The x value ""{leftInterval}"" in pluralization dictionary: ""{m_scope}"" culture: ""{m_cultureInfo.Name}""";
                            if (m_logger != null && m_logger.IsErrorEnabled())
                            {
                                m_logger.LogError(errorMessage);
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
                            if (m_logger != null && m_logger.IsErrorEnabled())
                            {
                                m_logger.LogError(errorMessage);
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

            var keyValueObjects = (JObject) m_jsonDictionary.SelectToken(ConstantJPath);
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