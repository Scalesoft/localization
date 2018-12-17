using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Impl;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Localization.CoreLibrary.Manager.Impl
{
    internal class FileDictionaryManager : IDictionaryManager
    {
        private readonly IConfiguration m_configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        public FileDictionaryManager(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        /// <summary>
        /// Returns HashSet with ALL loaded dictionaries.
        /// </summary>
        public HashSet<ILocalizationDictionary> Dictionaries { get; private set; }

        /// <summary>
        /// Automatically loads dictionary files based on folder structure in basePath (specified in library config).
        /// </summary>
        /// <param name="dictionaryFactory">Dictionary factory.</param>
        /// <returns>Array with loaded dictionaries.</returns>
        public ILocalizationDictionary[] AutoLoadDictionaries(IDictionaryFactory dictionaryFactory)
        {
            var localizationFilesToLoad = CheckResourceFiles(m_configuration, dictionaryFactory);

            var loadedDictionaries = new ILocalizationDictionary[localizationFilesToLoad.Count];
            for (var i = 0; i < loadedDictionaries.Length; i++)
            {
                loadedDictionaries[i] = dictionaryFactory.CreateDictionary(localizationFilesToLoad[i]);
            }

            return loadedDictionaries;
        }

        /// <summary>
        /// Check for resource files base on folders structure in basePath.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        /// <param name="dictionaryFactory"></param>
        /// <returns>List of resource files to load.</returns>
        private IList<string> CheckResourceFiles(IConfiguration configuration, IDictionaryFactory dictionaryFactory)
        {
            var fs = new FolderScanner(dictionaryFactory);
            return fs.CheckResourceFiles(configuration);
        }

        /// <summary>
        /// From provided dictionary instances builds hierarchical trees.
        /// </summary>
        /// <param name="dictionaries">Loaded dictionaries.</param>
        public void BuildDictionaryHierarchyTrees(IList<ILocalizationDictionary> dictionaries)
        {
            Dictionaries = new HashSet<ILocalizationDictionary>();

            foreach (var localizationDictionary in dictionaries)
            {
                if (
                    !localizationDictionary.CultureInfo().IsNeutralCulture
                    && localizationDictionary.CultureInfo().Name != m_configuration.DefaultCulture().Name
                )
                {
                    //Is not neutral and not default culture
                    Dictionaries.Add(localizationDictionary);
                }
            }

            foreach (var localizationDictionary in dictionaries)
            {
                foreach (var nonNeutralDictionary in Dictionaries)
                {
                    if (nonNeutralDictionary.Scope().Equals(localizationDictionary.Scope()))
                    {
                        if (nonNeutralDictionary.CultureInfo().Parent.Equals(localizationDictionary.CultureInfo()))
                        {
                            nonNeutralDictionary.SetParentDictionary(localizationDictionary);
                        }
                        else if (localizationDictionary.CultureInfo().Equals(m_configuration.DefaultCulture()))
                        {
                            nonNeutralDictionary.SetParentDictionary(localizationDictionary);
                        }
                    }
                }

                Dictionaries.Add(localizationDictionary);
            }
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

        public CultureInfo DefaultCulture()
        {
            return m_configuration.DefaultCulture();
        }

        public string DefaultScope()
        {
            return Localization.DefaultScope;
        }

        public ILocalizationDictionary GetLocalizationDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                scope = Localization.DefaultScope;
            }

            return GetScopedDictionary(cultureInfo, scope);
        }

        private ILocalizationDictionary GetScopedDictionary(CultureInfo cultureInfo, string scope)
        {
            if (Dictionaries == null)
            {
                Dictionaries = new HashSet<ILocalizationDictionary>();
            }

            ILocalizationDictionary result;
            if (IsCultureSupported(cultureInfo)) //return scoped dictionary in requested culture (if scope exists)
            {
                result = Dictionaries.FirstOrDefault(w => w.CultureInfo().Equals(cultureInfo) && w.Scope().Equals(scope));
            }
            else
            {
                //return scoped dictionary in default culture
                result = Dictionaries.FirstOrDefault(w =>
                    w.CultureInfo().Equals(m_configuration.DefaultCulture())
                    && w.Scope().Equals(scope)
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
            return m_configuration.DefaultCulture().Equals(cultureInfo) || m_configuration.SupportedCultures().Contains(cultureInfo);
        }
    }
}
