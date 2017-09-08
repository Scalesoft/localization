using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DatabaseLocalizationManager : LocalizationManager, ILocalizationManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        
        private readonly IDatabaseTranslateService m_dbTranslateService;

        public DatabaseLocalizationManager(IConfiguration configuration, IDatabaseTranslateService dbTranslateService) : base(configuration)
        {
            this.m_dbTranslateService = dbTranslateService;
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslate(text, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);


            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslateFormat(text, parameters, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslatePluralization(text, number, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslateConstant(text, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public CultureInfo DefaultCulture()
        {
            return Configuration.DefaultCulture();
        }

        public string DefaultScope()
        {
            return Localization.DefaultScope;
        }


        
    }
}