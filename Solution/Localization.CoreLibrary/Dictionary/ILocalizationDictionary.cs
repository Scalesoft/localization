using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Pluralization;

namespace Scalesoft.Localization.Core.Dictionary
{
    public interface ILocalizationDictionary
    {
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
        
        IList<string> ScopeAlias();

        string GetParentScopeName();

        /// <summary>
        /// Dictionary resource file extension. e.g "json", "xml".
        /// </summary>
        /// <returns>Resource file extension.</returns>
        string Extension();

        /// <summary>
        /// Dictionary.
        /// </summary>
        /// <returns>All key-value strings.</returns>
        IDictionary<string, LocalizedString> List();

        /// <summary>
        /// Dictionary.
        /// </summary>
        /// <returns>All pluralized strings.</returns>
        IDictionary<string, PluralizedString> ListPlurals();

        /// <summary>
        /// Dictionary
        /// </summary>
        /// <returns>All special constant strings.</returns>
        IDictionary<string, LocalizedString> ListConstants();

        /// <summary>
        /// Parent dictionary based on culture.
        /// </summary>
        /// <returns>Parent dictionary.</returns>
        ILocalizationDictionary ParentDictionary();

        /// <summary>
        /// Child dictionary based on culture.
        /// </summary>
        /// <returns>Child dictionary.</returns>
        IList<ILocalizationDictionary> ChildDictionaries { get; }

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
        void SetChildDictionary(ILocalizationDictionary childDictionary);

        /// <summary>
        /// Dictionary is leaf if it hasn't any child dictionaries in dictionary hierarchy.
        /// </summary>
        /// <returns>True if this dictionary is leaf in hierarchy.</returns>
        bool IsLeaf();

        bool IsRoot { get; }
    }
}
