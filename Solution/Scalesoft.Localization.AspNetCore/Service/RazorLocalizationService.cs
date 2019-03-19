using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Html;

namespace Scalesoft.Localization.AspNetCore.Service
{
    public class RazorLocalizationService : IRazorLocalizationService
    {
        private readonly ILocalizationService m_localizationService;

        public RazorLocalizationService(ILocalizationService localizationService)
        {
            m_localizationService = localizationService;
        }

        public HtmlString Translate(string text, string scope)
        {
            var translatedText = m_localizationService.Translate(text, scope);
            var encoded = HttpUtility.HtmlEncode(translatedText);

            return new HtmlString(encoded);
        }

        public HtmlString TranslateFormat(string text, string scope, params IHtmlContent[] parameters)
        {
            var translatedText = m_localizationService.Translate(text, scope);
            var encodedFormatted = GetFormattedEncodedText(translatedText, parameters);

            return new HtmlString(encodedFormatted);
        }

        public HtmlString TranslatePluralization(string text, int number, string scope)
        {
            var translatedText = m_localizationService.TranslatePluralization(text, number, scope);
            var encoded = HttpUtility.HtmlEncode(translatedText);

            return new HtmlString(encoded);
        }

        public HtmlString TranslateConstant(string text, string scope)
        {
            var translatedText = m_localizationService.TranslateConstant(text, scope);
            var encoded = HttpUtility.HtmlEncode(translatedText);

            return new HtmlString(encoded);
        }

        public HtmlString Translate(string text)
        {
            var translatedText = m_localizationService.Translate(text);
            var encoded = HttpUtility.HtmlEncode(translatedText);

            return new HtmlString(encoded);
        }

        public HtmlString TranslateFormat(string text, params IHtmlContent[] parameters)
        {
            var translatedText = m_localizationService.Translate(text);
            var encodedFormatted = GetFormattedEncodedText(translatedText, parameters);

            return new HtmlString(encodedFormatted);
        }

        public HtmlString TranslatePluralization(string text, int number)
        {
            var translatedText = m_localizationService.TranslatePluralization(text, number);
            var encoded = HttpUtility.HtmlEncode(translatedText);

            return new HtmlString(encoded);
        }

        public HtmlString TranslateConstant(string text)
        {
            var translatedText = m_localizationService.TranslateConstant(text);
            var encoded = HttpUtility.HtmlEncode(translatedText);

            return new HtmlString(encoded);
        }

        private string GetFormattedEncodedText(string text, IHtmlContent[] parameters)
        {
            var encoded = HttpUtility.HtmlEncode(text) ?? string.Empty;

            var stringParams = new List<object>();

            foreach (var htmlContent in parameters)
            {
                using (var writer = new System.IO.StringWriter())
                {
                    htmlContent.WriteTo(writer, HtmlEncoder.Default);
                    stringParams.Add(writer.ToString());
                }
            }

            var formatted = string.Format(encoded, stringParams.ToArray());

            return formatted;
        }
    }
}