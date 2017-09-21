using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Dictionary
{
    public interface ILocalizationDictionary
    {
        /// <summary>
        /// Loads dictionary from file.
        /// </summary>
        /// <param name="filePath">Resource file path.</param>
        /// <returns>This instance.</returns>
        ILocalizationDictionary Load(string filePath);
        /// <summary>
        /// Dictionary culture info.
        /// </summary>
        /// <returns>Dictionary culture info.</returns>
        CultureInfo CultureInfo();
        /// <summary>
        /// Dictionary scope.
        /// </summary>
        /// <returns>Dictionary scope.</returns>
        string Scope();
        /// <summary>
        /// Dictionary resource file extension. e.g "json", "xml".
        /// </summary>
        /// <returns>Resource file extension.</returns>
        string Extension();
        /// <summary>
        /// Dictionary.
        /// </summary>
        /// <returns>All key-value strings.</returns>
        Dictionary<string, LocalizedString> List();
        /// <summary>
        /// Dictionary.
        /// </summary>
        /// <returns>All pluralized strings.</returns>
        Dictionary<string, PluralizedString> ListPlurals();
        /// <summary>
        /// Dictionary
        /// </summary>
        /// <returns>All special constant strings.</returns>
        Dictionary<string, LocalizedString> ListConstants();
            /// <summary>
        /// Parent dictionary based on culture hierarchy.
        /// </summary>
        /// <returns>Parent dictionary.</returns>
        ILocalizationDictionary ParentDictionary();
        /// <summary>
        /// Child dictionary based on culture hierarchy.
        /// </summary>
        /// <returns>Child dictionary.</returns>
        ILocalizationDictionary ChildDictionary();
        /// <summary>
        /// Sets parent dictionary for this.
        /// </summary>
        /// <param name="parentDictionary"></param>
        /// <returns>False if parent dictionary is already set.</returns>
        bool SetParentDictionary(ILocalizationDictionary parentDictionary);
        /// <summary>
        /// Set child dictionary for this.
        /// </summary>
        /// <param name="childDictionary">Child dictionary.</param>
        /// <returns>False if child dictionary is already set.</returns>
        bool SetChildDictionary(ILocalizationDictionary childDictionary);
        /// <summary>
        /// Dictionary is leaf if it hasn't any child dictionaries in dictionary hieararchy.
        /// </summary>
        /// <returns>True if this dictionary is leaf in hierarchy.</returns>
        bool IsLeaf();

        bool IsRoot { get; set; }
    }
}