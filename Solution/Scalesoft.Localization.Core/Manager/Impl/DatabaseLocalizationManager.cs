using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Model;
using Scalesoft.Localization.Core.Resolver;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class DatabaseLocalizationManager : LocalizationManagerBase, IDatabaseLocalizationManager, IDatabaseDynamicTextService
    {
        private readonly IDatabaseTranslateService m_dbTranslateService;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;
        private readonly FallbackCultureResolver m_fallbackCultureResolver;

        public DatabaseLocalizationManager(
            LocalizationConfiguration configuration,
            IDatabaseTranslateService dbTranslateService,
            IDatabaseDynamicTextService databaseDynamicTextService,
            FallbackCultureResolver fallbackCultureResolver
        ) : base(configuration)
        {
            m_dbTranslateService = dbTranslateService;
            m_databaseDynamicTextService = databaseDynamicTextService;
            m_fallbackCultureResolver = fallbackCultureResolver;

            Check();
        }

        private void Check()
        {
            m_dbTranslateService.CheckCulturesInDatabase();
        }

        public LocalizedString Translate(CultureInfo cultureInfo, string scope, string text)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            while (true)
            {
                var resultLocalizedString = m_dbTranslateService.DatabaseTranslate(text, cultureInfo, scope);

                if (resultLocalizedString != null)
                {
                    return resultLocalizedString;
                }

                cultureInfo = cultureInfo == null ? null : m_fallbackCultureResolver.FallbackCulture(cultureInfo);
                if (cultureInfo != null)
                {
                    continue;
                }

                return TranslateFallback(text);
            }
        }

        public LocalizedString TranslateFormat(CultureInfo cultureInfo,
            string scope, string text, object[] parameters)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            var resultLocalizedString = m_dbTranslateService.DatabaseTranslateFormat(text, parameters, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslatePluralization(CultureInfo cultureInfo,
            string scope, string text, int number)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            var resultLocalizedString = m_dbTranslateService.DatabaseTranslatePluralization(text, number, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslateConstant(CultureInfo cultureInfo, string scope, string text)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            var resultLocalizedString = m_dbTranslateService.DatabaseTranslateConstant(text, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            return m_databaseDynamicTextService.GetDynamicText(name, scope, cultureInfo);
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            return m_databaseDynamicTextService.GetAllDynamicText(name, scope);
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText,
            IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing)
        {
            return m_databaseDynamicTextService.SaveDynamicText(dynamicText, actionForDefaultCulture);
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            m_databaseDynamicTextService.DeleteDynamicText(name, scope, cultureInfo);
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            m_databaseDynamicTextService.DeleteAllDynamicText(name, scope);
        }
    }
}