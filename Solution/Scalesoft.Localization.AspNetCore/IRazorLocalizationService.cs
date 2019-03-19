using Microsoft.AspNetCore.Html;

namespace Scalesoft.Localization.AspNetCore
{
    public interface IRazorLocalizationService
    {
        /// <summary>
        /// Translates text using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized HtmlString with <b>encoded</b> content.</returns>
        HtmlString Translate(string text, string scope);

        /// <summary>
        /// Translates text with parameters using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="parameters">List of IHtmlContent elements</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized HtmlString with encoded content and input parameter.</returns>
        HtmlString TranslateFormat(string text, string scope, params IHtmlContent[] parameters);

        /// <summary>
        /// Translates text, which has to be pluralized (declined) using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="number">Integer value defines what value will be chosen.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized pluralized HtmlString with encoded content.</returns>
        HtmlString TranslatePluralization(string text, int number, string scope);

        /// <summary>
        /// Translates constants using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="scope">String name of a scope in dictionary.</param>
        /// <returns>Localized constant HtmlString with encoded content.</returns>
        HtmlString TranslateConstant(string text, string scope);

        /// <summary>
        /// Translates text using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <returns>Localized HtmlString with encoded content.</returns>
        HtmlString Translate(string text);

        /// <summary>
        /// Translates text with parameters using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="parameters">List of IHtmlContent elements</param>
        /// <returns>Localized HtmlString with encoded content and input parameter.</returns>
        HtmlString TranslateFormat(string text, params IHtmlContent[] parameters);

        /// <summary>
        /// Translates text, which has to be pluralized (declined) using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <param name="number">Integer value defines what value will be chosen.</param>
        /// <returns>Localized pluralized HtmlString with encoded content.</returns>
        HtmlString TranslatePluralization(string text, int number);

        /// <summary>
        /// Translates constants using ILocalizationService and returns HtmlString with <strong>encoded</strong> translation.
        /// </summary>
        /// <param name="text">String key for translation in json resource file.</param>
        /// <returns>Localized constant HtmlString with encoded content.</returns>
        HtmlString TranslateConstant(string text);

    }
}