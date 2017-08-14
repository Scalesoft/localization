using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DictionaryManager : IDictionaryManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private readonly IConfiguration m_configuration;
        private const string GlobalDictionaryScope = "global";
        private HashSet<ILocalizationDictionary> m_dictionaries;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">Library configuration.</param>
        public DictionaryManager(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        /// <summary>
        /// Returns HashSet with ALL loaded dictionaries.
        /// </summary>
        public HashSet<ILocalizationDictionary> Dictionaries
        {
            get { return m_dictionaries; }
        }

        /// <summary>
        /// Automatically loads dictionary files based on folder structure in basePath (specified in library config).
        /// </summary>
        /// <param name="dictionaryFactory">Dictionary factory.</param>
        /// <returns>Array with loaded dictionaries.</returns>
        public ILocalizationDictionary[] AutoLoadDictionaries(IDictionaryFactory dictionaryFactory)
        {
            IList<string> localizationFilesToLoad = CheckResourceFiles(m_configuration, dictionaryFactory);

            ILocalizationDictionary[] loadedDictionaries = new ILocalizationDictionary[localizationFilesToLoad.Count];
            for (int i = 0; i < loadedDictionaries.Length; i++)
            {
                loadedDictionaries[i] = dictionaryFactory.CreateDictionary().Load(localizationFilesToLoad[i]);
            }

            return loadedDictionaries;
        }

        /// <summary>
        /// Check for resource files base on folders structure in basePath. 
        /// </summary>
        /// <param name="configuration">Libary configuration.</param>
        /// <returns>List of resource files to load.</returns>
        private IList<string> CheckResourceFiles(IConfiguration configuration, IDictionaryFactory dictionaryFactory)
        {
            FolderScanner fs = new FolderScanner(dictionaryFactory);
            return fs.CheckResourceFiles(configuration);
        }

        /// <summary>
        /// From provided dictionary instances builds hiararchical trees.
        /// </summary>
        /// <param name="dictionaries">Loaded dictionaries.</param>
        public void BuildDictionaryHierarchyTrees(ILocalizationDictionary[] dictionaries)
        {
            m_dictionaries = new HashSet<ILocalizationDictionary>();

            foreach (ILocalizationDictionary localizationDictionary in dictionaries)
            {
                if (!localizationDictionary.CultureInfo().IsNeutralCulture)
                {
                    m_dictionaries.Add(localizationDictionary);
                }
            }
            foreach (ILocalizationDictionary localizationDictionary in dictionaries)               
            {
                foreach (ILocalizationDictionary nonNeutralDictionary in m_dictionaries)
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
                m_dictionaries.Add(localizationDictionary);
            }
        }

        public Dictionary<string,LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).List();
        }

        public Dictionary<string, LocalizedString> GetDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionaryPart(part, cultureInfo, scope).List();
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).ListPlurals();
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            return GetLocalizationDictionary(cultureInfo, scope).ListConstants();//TODO List() -> ListConstants()
        }

        public ILocalizationDictionary GetLocalizationDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                scope = GlobalDictionaryScope;
            }

            return GetScopedDictionary(cultureInfo, scope);
        }

        //TODO: PART
        public ILocalizationDictionary GetLocalizationDictionaryPart(string part, CultureInfo cultureInfo = null, string scope = null)
        {
            if (scope == null)
            {
                scope = GlobalDictionaryScope;
            }

            throw new NotImplementedException();
        }

        private ILocalizationDictionary GetScopedDictionary(CultureInfo cultureInfo, string scope)
        {
            if (IsCultureSupported(cultureInfo)) //return scoped dictionary in requested culture (if scope exists)
            {              
                return m_dictionaries.First(w => w.CultureInfo().Equals(cultureInfo) && w.Scope().Equals(scope));
            }

            //return scoped dictionary in default culture
            return m_dictionaries.First(w => w.CultureInfo().Equals(m_configuration.DefaultCulture()) && w.Scope().Equals(scope));
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