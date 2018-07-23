using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface IDictionaryManager
    {
        /// <summary>
        /// Gets dictionary with keys and localized values.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Dictionary with keys and localized values.</returns>
        Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null);

        /// <summary>
        /// Gets pluralized dictionary with keys and localized values.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Pluralized dictionary with keys and localized values.</returns>
        Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null);

        /// <summary>
        /// Gets dictionary with keys and localized constant values.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Dictionary with keys and localized constant values.</returns>
        Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null);

        CultureInfo DefaultCulture();

        string DefaultScope();
    }
}