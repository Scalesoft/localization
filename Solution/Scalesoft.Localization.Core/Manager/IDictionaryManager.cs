using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Pluralization;

namespace Scalesoft.Localization.Core.Manager
{
    public interface IDictionaryManager
    {
        /// <summary>
        /// Gets dictionary with keys and localized values.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Dictionary with keys and localized values.</returns>
        IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null);

        /// <summary>
        /// Gets pluralized dictionary with keys and localized values.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Pluralized dictionary with keys and localized values.</returns>
        IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null);

        /// <summary>
        /// Gets pluralized dictionary with keys and localized values for the puprpose of client-side translation.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Pluralized dictionary with keys and localized values.</returns>
        IDictionary<string, ClientPluralizedString> GetClientPluralizedDictionary(CultureInfo cultureInfo = null,
            string scope = null);

        /// <summary>
        /// Gets dictionary with keys and localized constant values.
        /// </summary>
        /// <param name="cultureInfo">Culture info includes names, calendars, date formatting etc. Default values is configured in localization config file.</param>
        /// <param name="scope">String name of a scope. Dictionary of this scope will be returned. Default value is global.</param>
        /// <returns>Dictionary with keys and localized constant values.</returns>
        IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null);

        CultureInfo DefaultCulture();

        string DefaultScope();
    }
}