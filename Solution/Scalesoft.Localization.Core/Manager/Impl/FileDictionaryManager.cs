using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Dictionary.Impl;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Util.Impl;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class FileDictionaryManager : ManagerBase, IFileDictionaryManager
    {
        private const string UnknownCultureException = "Unknown culture {0} with scope {1}";

        private const string GlobalScope = "global";

        private readonly ISet<ILocalizationDictionary> m_dictionaries;
        private readonly IDictionary<CultureInfo, IDictionary<string, ILocalizationDictionary>> m_dictionariesPerCultureAndScope;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory"></param>
        /// <param name="logger"></param>
        public FileDictionaryManager(
            LocalizationConfiguration configuration,
            IDictionaryFactory dictionaryFactory,
            ILogger<FileDictionaryManager> logger = null
        ) : base(configuration, logger)
        {
            m_dictionaries = new HashSet<ILocalizationDictionary>();
            m_dictionariesPerCultureAndScope = new ConcurrentDictionary<CultureInfo, IDictionary<string, ILocalizationDictionary>>
            {
                [GetDefaultCulture()] = new ConcurrentDictionary<string, ILocalizationDictionary>()
            };

            foreach (var supportedCulture in m_configuration.SupportedCultures)
            {
                if (!m_dictionariesPerCultureAndScope.ContainsKey(supportedCulture))
                {
                    m_dictionariesPerCultureAndScope.Add(supportedCulture, new ConcurrentDictionary<string, ILocalizationDictionary>());
                }
            }

            if (configuration.AutoLoadResources)
            {
                AutoLoadDictionaries(dictionaryFactory);
            }
        }

        private void CheckGlobalScopeAvailabilityInAllCulture()
        {
            foreach (var dictionariesPerScope in m_dictionariesPerCultureAndScope)
            {
                if (!dictionariesPerScope.Value.Keys.Contains(GlobalScope))
                {
                    throw new DictionaryLoadException(
                        $"Not found '{GlobalScope}' scope in '{dictionariesPerScope.Key.Name}' culture, unable to construct dictionary tree");
                }
            }
        }

        /// <summary>
        /// Automatically loads dictionary files based on folder structure in basePath (specified in library config).
        /// </summary>
        /// <param name="dictionaryFactory">Dictionary factory.</param>
        public void AutoLoadDictionaries(IDictionaryFactory dictionaryFactory)
        {
            var localizationFilesToLoad = CheckResourceFiles(m_configuration, dictionaryFactory);

            foreach (var loadedDictionary in localizationFilesToLoad)
            {
                AddDictionaryToHierarchyTreesWithoutBuildTree(dictionaryFactory.CreateDictionary(loadedDictionary));
            }

            CheckGlobalScopeAvailabilityInAllCulture();

            BuildDictionaryHierarchyTrees();
        }

        /// <summary>
        /// Check for resource files base on folders structure in basePath.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory"></param>
        /// <returns>List of resource files to load.</returns>
        private IEnumerable<string> CheckResourceFiles(LocalizationConfiguration configuration, IDictionaryFactory dictionaryFactory)
        {
            var fs = new FolderScanner(dictionaryFactory, configuration);

            return fs.GetAllDictionaryFullpaths(configuration.BasePath);
        }

        public void AddDictionaryToHierarchyTrees(ILocalizationDictionary dictionary)
        {
            AddDictionaryToHierarchyTreesWithoutBuildTree(dictionary);

            BuildDictionaryHierarchyTrees(m_dictionariesPerCultureAndScope[dictionary.CultureInfo()], dictionary);
        }

        private void AddDictionaryToHierarchyTreesWithoutBuildTree(ILocalizationDictionary dictionary)
        {
            m_dictionaries.Add(dictionary);

            if (!m_dictionariesPerCultureAndScope.Keys.Contains(dictionary.CultureInfo()))
            {
                // Ignore unsupported languages without any error
                return;
            }

            var dictionariesPerCulture = m_dictionariesPerCultureAndScope[dictionary.CultureInfo()];

            TryAddDictionaryKey(dictionariesPerCulture, dictionary.Scope(), dictionary);

            foreach (var scopeAlias in dictionary.ScopeAlias())
            {
                TryAddDictionaryKey(dictionariesPerCulture, scopeAlias, dictionary);
            }
        }

        private void TryAddDictionaryKey<TK,TV>(IDictionary<TK,TV> dictionary, TK key, TV value)
        {
            try
            {
                dictionary.Add(key, value);
            }
            catch (ArgumentNullException e)
            {
                throw new DictionaryLoadException("Cannot add null key to the dictionary", e);
            }
            catch (ArgumentException e)
            {
                throw new DictionaryLoadException($"Key {key} already exists in the dictionary", e);
            }
        }

        private void BuildDictionaryHierarchyTrees()
        {
            foreach (var dictionaries in m_dictionariesPerCultureAndScope)
            {
                BuildDictionaryHierarchyTrees(dictionaries.Value);
            }
        }

        /// <summary>
        /// From provided dictionary instances builds hierarchical trees.
        /// </summary>
        private void BuildDictionaryHierarchyTrees(IDictionary<string, ILocalizationDictionary> dictionaries)
        {
            var global = dictionaries[GlobalScope];

            foreach (var cultureDictionary in dictionaries.Values)
            {
                if (
                    cultureDictionary.ParentDictionary() != null
                    || cultureDictionary.Equals(global)
                )
                {
                    continue;
                }

                var parentScope = string.IsNullOrEmpty(cultureDictionary.GetParentScopeName())
                    ? global
                    : dictionaries[cultureDictionary.GetParentScopeName()];

                cultureDictionary.SetParentDictionary(parentScope);
            }
        }

        private void BuildDictionaryHierarchyTrees(IDictionary<string, ILocalizationDictionary> dictionaries,
            ILocalizationDictionary dictionary)
        {
            if (dictionary.Scope() == GlobalScope)
            {
                return;
            }

            var parentScope = string.IsNullOrEmpty(dictionary.GetParentScopeName())
                ? dictionaries[GlobalScope]
                : dictionaries[dictionary.GetParentScopeName()];

            dictionary.SetParentDictionary(parentScope);
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).List();
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).ListPlurals();
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).ListConstants();
        }

        public ILocalizationDictionary GetLocalizationDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                scope = GetDefaultScope();
            }

            return GetScopedDictionary(cultureInfo, scope);
        }

        private ILocalizationDictionary GetScopedDictionary(CultureInfo cultureInfo, string scope)
        {
            var dictionaryPerCulture = m_dictionariesPerCultureAndScope[
                IsCultureSupported(cultureInfo) ? cultureInfo : GetDefaultCulture()
            ];

            dictionaryPerCulture.TryGetValue(scope, out var result);

            return result ?? new EmptyLocalizationDictionary();
        }


        /// <summary>
        /// Returns true if given culture is default culture or it is in supported cultures.
        /// </summary>
        /// <param name="cultureInfo">Culture to check</param>
        /// <returns>
        /// Returns true if given culture is default culture or it is in supported cultures.
        /// If given culture is null method returns false.
        /// </returns>
        public bool IsCultureSupported(CultureInfo cultureInfo)
        {
            return GetDefaultCulture().Equals(cultureInfo) || m_configuration.SupportedCultures.Contains(cultureInfo);
        }
    }
}
