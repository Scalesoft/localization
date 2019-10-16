using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.AspNetCore.Service
{
    public sealed class LocalizationService : ServiceBase, ILocalizationService
    {
        private readonly IAutoLocalizationManager m_localizationManager;

        public LocalizationService(
            IRequestCultureManager requestCultureManager,
            IAutoLocalizationManager autoLocalizationManager
        ) : base(requestCultureManager)
        {
            m_localizationManager = autoLocalizationManager;
        }

        public CultureInfo GetRequestCulture()
        {
            return GetRequestCulture(m_localizationManager.GetDefaultCulture());
        }

        //Explicit calls
        public CultureInfo[] GetSupportedCultures()
        {
            return m_localizationManager.GetSupportedCultures();
        }

        public void SetCulture(string culture)
        {
            m_requestCultureManager.SetCulture(culture);
        }

        public void SetResponseCookie()
        {
            m_requestCultureManager.SetResponseCookie();
        }

        public LocalizedString Translate(string text, string scope, LocTranslationSource translationSource)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.Translate(translationSource, requestCulture, scope, text);
        }

        public LocalizedString TranslateFormat(string text, string scope, LocTranslationSource translationSource, params object[] parameters)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslateFormat(translationSource, requestCulture, scope, text, parameters);
        }

        public LocalizedString TranslatePluralization(string text, string scope, LocTranslationSource translationSource, int number)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslatePluralization(translationSource, requestCulture, scope, text, number);
        }

        public LocalizedString TranslateConstant(string text, string scope, LocTranslationSource translationSource)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslateConstant(translationSource, requestCulture, scope, text);
        }

        //Explicit calls and translationSource = LocTranslationSource.Auto
        public LocalizedString Translate(string text, string scope)
        {
            return Translate(text, scope, LocTranslationSource.Auto);
        }
        
        public LocalizedString TranslateFormat(string text, string scope, params object[] parameters)
        {
            return TranslateFormat(text, scope, LocTranslationSource.Auto, parameters);
        }

        public LocalizedString TranslatePluralization(string text, string scope, int number)
        {
            return TranslatePluralization(text, scope, LocTranslationSource.Auto, number);
        }

        public LocalizedString TranslateConstant(string text, string scope)
        {
            return TranslateConstant(text, scope, LocTranslationSource.Auto);
        }


        //Without scope
        public LocalizedString Translate(string text, LocTranslationSource translationSource)
        {
            return Translate(text, null, translationSource);
        }
        
        public LocalizedString TranslateFormat(string text, LocTranslationSource translationSource, params object[] parameters)
        {
            return TranslateFormat(text, null, translationSource, parameters);
        }

        public LocalizedString TranslatePluralization(string text,
            LocTranslationSource translationSource, int number)
        {
            return TranslatePluralization(text, null, translationSource, number);
        }

        public LocalizedString TranslateConstant(string text, LocTranslationSource translationSource)
        {
            return TranslateConstant(text, null, translationSource);
        }

        //Without scope and translationSource = LocTranslationSource.Auto
        public LocalizedString Translate(string text)
        {
            return Translate(text, null, LocTranslationSource.Auto);
        }

        public LocalizedString TranslateFormat(string text, params object[] parameters)
        {
            return TranslateFormat(text, null, LocTranslationSource.Auto, parameters);
        }

        public LocalizedString TranslatePluralization(string text, int number)
        {
            return TranslatePluralization(text, null, LocTranslationSource.Auto, number);
        }

        public LocalizedString TranslateConstant(string text)
        {
            return TranslateConstant(text, null, LocTranslationSource.Auto);
        }
    }
}
