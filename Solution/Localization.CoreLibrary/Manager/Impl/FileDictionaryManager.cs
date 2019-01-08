using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Impl;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class FileDictionaryManager : ManagerBase, IFileDictionaryManager
    {
        private const string UnknownCultureException = "Unknown culture {0} with scope {1}";

        private const string GlobalScope = "global";

        private readonly ISet<ILocalizationDictionary> m_dictionaries;
        private readonly IDictionary<CultureInfo, ISet<ILocalizationDictionary>> m_dictionariesPerCulture;

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
            m_dictionariesPerCulture = new Dictionary<CultureInfo, ISet<ILocalizationDictionary>>
            {
                {DefaultCulture(), new HashSet<ILocalizationDictionary>()}
            };

            foreach (var supportedCulture in m_configuration.SupportedCultures)
            {
                if (!m_dictionariesPerCulture.ContainsKey(supportedCulture))
                {
                    m_dictionariesPerCulture.Add(supportedCulture, new HashSet<ILocalizationDictionary>());
                }
            }

            if (configuration.AutoLoadResources)
            {
                AutoLoadDictionaries(dictionaryFactory);
            }
        }

        /// <summary>
        /// Automatically loads dictionary files based on folder structure in basePath (specified in library config).
        /// </summary>
        /// <param name="dictionaryFactory">Dictionary factory.</param>
        private void AutoLoadDictionaries(IDictionaryFactory dictionaryFactory)
        {
            var localizationFilesToLoad = CheckResourceFiles(m_configuration, dictionaryFactory);

            foreach (var loadedDictionary in localizationFilesToLoad)
            {
                AddDictionaryToHierarchyTreesWithoutBuildTree(dictionaryFactory.CreateDictionary(loadedDictionary));
            }

            BuildDictionaryHierarchyTrees();
        }

        /// <summary>
        /// Check for resource files base on folders structure in basePath.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory"></param>
        /// <returns>List of resource files to load.</returns>
        private IEnumerable<string> CheckResourceFiles(LocalizationConfiguration configuration,
            IDictionaryFactory dictionaryFactory)
        {
            var fs = new FolderScanner(dictionaryFactory);
            return fs.CheckResourceFiles(configuration);
        }

        public void AddDictionaryToHierarchyTrees(ILocalizationDictionary dictionary)
        {
            AddDictionaryToHierarchyTreesWithoutBuildTree(dictionary);

            BuildDictionaryHierarchyTrees(m_dictionariesPerCulture[dictionary.CultureInfo()], dictionary);
        }

        private void AddDictionaryToHierarchyTreesWithoutBuildTree(ILocalizationDictionary dictionary)
        {
            m_dictionaries.Add(dictionary);

            if (!m_dictionariesPerCulture.Keys.Contains(dictionary.CultureInfo()))
            {
                throw new DictionaryLoadException(string.Format(UnknownCultureException, dictionary.CultureInfo(),
                    dictionary.Scope()));
            }

            m_dictionariesPerCulture[dictionary.CultureInfo()].Add(dictionary);
        }

        private void BuildDictionaryHierarchyTrees()
        {
            foreach (var dictionaries in m_dictionariesPerCulture)
            {
                BuildDictionaryHierarchyTrees(dictionaries.Value);
            }
        }

        /// <summary>
        /// From provided dictionary instances builds hierarchical trees.
        /// </summary>
        private void BuildDictionaryHierarchyTrees(ISet<ILocalizationDictionary> dictionaries)
        {
            var global = dictionaries.FirstOrDefault(d => d.Scope() == GlobalScope);

            foreach (var cultureDictionary in dictionaries)
            {
                if (
                    cultureDictionary.ParentDictionary() != null
                    || Equals(cultureDictionary, global)
                )
                {
                    continue;
                }

                var parentScope = string.IsNullOrEmpty(cultureDictionary.GetParentScopeName())
                    ? null
                    : dictionaries.FirstOrDefault(d => d.Scope() == cultureDictionary.GetParentScopeName());

                cultureDictionary.SetParentDictionary(
                    parentScope ?? global
                );
            }
        }

        private void BuildDictionaryHierarchyTrees(ISet<ILocalizationDictionary> dictionaries,
            ILocalizationDictionary dictionary)
        {
            if (dictionary.Scope() == GlobalScope)
            {
                return;
            }

            var parentScope = string.IsNullOrEmpty(dictionary.GetParentScopeName())
                ? null
                : dictionaries.FirstOrDefault(d => d.Scope() == dictionary.GetParentScopeName());

            dictionary.SetParentDictionary(
                parentScope ?? dictionaries.FirstOrDefault(d => d.Scope() == GlobalScope)
            );
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).List();
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).ListPlurals();
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null,
            string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).ListConstants();
        }

        public ILocalizationDictionary GetLocalizationDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                scope = DefaultScope();
            }

            return GetScopedDictionary(cultureInfo, scope);
        }

        private ILocalizationDictionary GetScopedDictionary(CultureInfo cultureInfo, string scope)
        {
            ILocalizationDictionary result;
            if (IsCultureSupported(cultureInfo)) //return scoped dictionary in requested culture (if scope exists)
            {
                result = m_dictionariesPerCulture[cultureInfo].FirstOrDefault(
                    w => w.Scope().Equals(scope) || w.ScopeAlias().Contains(scope)
                );
            }
            else
            {
                //return scoped dictionary in default culture
                result = m_dictionariesPerCulture[DefaultCulture()].FirstOrDefault(
                    w => w.Scope().Equals(scope) || w.ScopeAlias().Contains(scope)
                );
            }

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
            return DefaultCulture().Equals(cultureInfo) || m_configuration.SupportedCultures.Contains(cultureInfo);
        }
    }
}