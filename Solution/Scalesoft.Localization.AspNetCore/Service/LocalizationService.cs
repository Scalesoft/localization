using System;
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

        public LocalizedString Translate(string text, string scope, LocTranslationSource translationSource)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.Translate(translationSource, text, requestCulture, scope);
        }

        [Obsolete("Use new method with params")]
        public LocalizedString TranslateFormat(string text, object[] parameters, string scope, LocTranslationSource translationSource)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslateFormat(translationSource, text, parameters, requestCulture, scope);
        }

        public LocalizedString TranslateFormat(string text, string scope, LocTranslationSource translationSource, params object[] parameters)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslateFormat(translationSource, text, scope, requestCulture, parameters);
        }

        public LocalizedString TranslatePluralization(string text, int number, string scope, LocTranslationSource translationSource)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslatePluralization(translationSource, text, number, requestCulture, scope);
        }

        public LocalizedString TranslateConstant(string text, string scope, LocTranslationSource translationSource)
        {
            var requestCulture = GetRequestCulture();

            return m_localizationManager.TranslateConstant(translationSource, text, requestCulture, scope);
        }

        //Explicit calls and translationSource = LocTranslationSource.Auto
        public LocalizedString Translate(string text, string scope)
        {
            return Translate(text, scope, LocTranslationSource.Auto);
        }

        [Obsolete("Use new method with params")]
        public LocalizedString TranslateFormat(string text, object[] parameters, string scope)
        {
            return TranslateFormat(text, parameters, scope, LocTranslationSource.Auto);
        }

        public LocalizedString TranslateFormat(string text, string scope, params object[] parameters)
        {
            return TranslateFormat(text, scope, LocTranslationSource.Auto, parameters);
        }

        public LocalizedString TranslatePluralization(string text, int number,
            string scope)
        {
            return TranslatePluralization(text, number, scope, LocTranslationSource.Auto);
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

        [Obsolete("Use new method with params")]
        public LocalizedString TranslateFormat(string text, object[] parameters, LocTranslationSource translationSource)
        {
            return TranslateFormat(text, parameters, null, translationSource);
        }

        public LocalizedString TranslateFormat(string text, LocTranslationSource translationSource, params object[] parameters)
        {
            return TranslateFormat(text, null, translationSource, parameters);
        }

        public LocalizedString TranslatePluralization(string text, int number,
            LocTranslationSource translationSource)
        {
            return TranslatePluralization(text, number, null, translationSource);
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
            return TranslatePluralization(text, number, null, LocTranslationSource.Auto);
        }

        public LocalizedString TranslateConstant(string text)
        {
            return TranslateConstant(text, null, LocTranslationSource.Auto);
        }
    }
}
