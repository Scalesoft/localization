using System;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.AspNetCore
{
    public interface ILocalizationService
    {
        CultureInfo[] GetSupportedCultures();

        /// <summary>
        /// Gets current culture.
        /// </summary>
        /// <returns></returns>
        CultureInfo GetRequestCulture();

        /// <summary>
        /// Changes language.
        /// </summary>
        /// <param name="culture"></param>
        void SetCulture(string culture);

        //Explicit calls
        /// <summary>
        /// Translates text using a resource file (.json) or a database.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized string.</returns>
        LocalizedString Translate(string text, string scope, LocTranslationSource translationSource);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="parameters">Object with parameters. Parameters can be of any type except another object.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized string with input parameter.</returns>
        [Obsolete("Use new method with params")]
        LocalizedString TranslateFormat(string text, object[] parameters, string scope, LocTranslationSource translationSource);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param> 
        /// <param name="parameters">List of parameters. Parameters can be of any type.</param>
        /// <returns>Localized string with input parameter.</returns>
        LocalizedString TranslateFormat(string text, string scope, LocTranslationSource translationSource, params object[] parameters);

        /// <summary>
        /// Translates text, which has to be pluralized (declined). Using a resource file (.json) or a database.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="number">Integer value defines what value will be chosen.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized pluralized string.</returns>
        LocalizedString TranslatePluralization(string text, int number, string scope, LocTranslationSource translationSource);

        /// <summary>
        /// Translates constants using a resource file (.json) or a database.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized constant string.</returns>
        LocalizedString TranslateConstant(string text, string scope, LocTranslationSource translationSource);

        //Explicit calls and translationSource = LocTranslationSource.Auto
        /// <summary>
        /// Translates text using a resource file (.json) or a database.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized string.</returns>
        LocalizedString Translate(string text, string scope);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database. Translation source is Auto.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="parameters">Object with parameters. Parameters can be of any type except another object.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized string with input parameter.</returns>
        [Obsolete("Use new method with params")]
        LocalizedString TranslateFormat(string text, object[] parameters, string scope);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database. Translation source is Auto.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="parameters">List of parameters. Parameters can be of any type.</param>
        /// <returns>Localized string with input parameter.</returns> 
        LocalizedString TranslateFormat(string text, string scope, params object[] parameters);

        /// <summary>
        /// Translates text, which has to be pluralized (declined). using a resource file (.json) or a database. Translation source is Auto.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <param name="number">Integer value defines what value will be chosen.</param>
        /// <returns>Localized pluralized string.</returns>
        LocalizedString TranslatePluralization(string text, string scope, int number);

        /// <summary>
        /// Translates constants using a resource file (.json) or a database. Translation source is Auto. 
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized constant string.</returns>
        LocalizedString TranslateConstant(string text, string scope);
        
        //Without scope
        /// <summary>
        /// Translates text using a resource file (.json) or a database. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized string.</returns>
        LocalizedString Translate(string text, LocTranslationSource translationSource);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="parameters">Object with parameters. Parameters can be of any type except another object.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized string with input parameter.</returns>
        [Obsolete("Use new method with params")]
        LocalizedString TranslateFormat(string text, object[] parameters, LocTranslationSource translationSource);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param> 
        /// <param name="parameters">List of parameters. Parameters can be of any type.</param>
        /// <returns>Localized string with input parameter.</returns>
        LocalizedString TranslateFormat(string text, LocTranslationSource translationSource, params object[] parameters);
        
        /// <summary>
        /// Translates text, which has to be pluralized (declined). Using a resource file (.json) or a database. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="number">Integer value defines what value will be chosen.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized pluralized string.</returns>
        LocalizedString TranslatePluralization(string text, int number, LocTranslationSource translationSource);

        /// <summary>
        /// Translates constants using a resource file (.json) or a database. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="translationSource">Source file for translation. Values Auto - choose automatically, Database - dictionary from database, File - dictionary from local json file.</param>
        /// <returns>Localized constant string.</returns>
        LocalizedString TranslateConstant(string text, LocTranslationSource translationSource);

        //Without scope and translationSource = LocTranslationSource.Auto
        /// <summary>
        /// Translates text using a resource file (.json) or a database. Translation source is Auto. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <returns>Localized string.</returns>
        LocalizedString Translate(string text);

        /// <summary>
        /// Translates text with parameters using a resource file (.json) or a database. Translation source is Auto. Scope is default - global. 
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="parameters">List of parameters. Parameters can be of any type.</param>
        /// <returns>Localized string with input parameter.</returns>
        LocalizedString TranslateFormat(string text, params object[] parameters);

        /// <summary>
        /// Translates text, which has to be pluralized (declined) using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="number">Integer value defines what value will be chosen.</param>
        /// <returns>Localized pluralized HtmlString with encoded content.</returns>
        LocalizedString TranslatePluralization(string text, int number);

        /// <summary>
        /// Translates constants using a resource file (.json) or a database. Translation source is Auto. Scope is default - global.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <returns>Localized constant string.</returns>
        LocalizedString TranslateConstant(string text);
    }

    [Obsolete("ILocalization is replaced by ILocalizationService")]
    public interface ILocalization { }
}