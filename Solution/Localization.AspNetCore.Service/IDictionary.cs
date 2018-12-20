using System.Collections.Generic;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    public interface IDictionary
    {
        /// <summary>
        /// Gets dictionary with keys and localized values.
        /// </summary>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <param name="translationSource">Source of a dictionary. Can be file or database.</param>
        /// <returns>Dictionary with keys and localized values.</returns>
        IDictionary<string, LocalizedString> GetDictionary(
            string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto
        );

        /// <summary>
        /// Gets pluralized dictionary with keys and localized values.
        /// </summary>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <param name="translationSource">Source of a dictionary. Can be file or database.</param>
        /// <returns>Pluralized dictionary with keys and localized values.</returns>
        IDictionary<string, PluralizedString> GetPluralizedDictionary(
            string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto
        );

        /// <summary>
        /// Gets dictionary with keys and localized constant values.
        /// </summary>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <param name="translationSource">Source of a dictionary. Can be file or database.</param>
        /// <returns>Dictionary with keys and localized constant values.</returns>
        IDictionary<string, LocalizedString> GetConstantsDictionary(
            string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto
        );
    }
}