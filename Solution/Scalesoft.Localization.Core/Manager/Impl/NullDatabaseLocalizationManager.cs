using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Logging;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class NullDatabaseLocalizationManager : IDatabaseLocalizationManager
    {
        private readonly ILogger m_logger;
        private const string NotSetMessage = "Database localization manager is not set.";

        public NullDatabaseLocalizationManager(ILogger<NullDatabaseLocalizationManager> logger = null)
        {
            m_logger = logger;
        }

        private void LogNotSet()
        {
            if (m_logger != null && m_logger.IsInformationEnabled())
            {
                m_logger.LogInformation(NotSetMessage);
            }
        }

        public LocalizedString Translate(CultureInfo cultureInfo, string scope, string text)
        {
            LogNotSet();

            return null;
        }

        public LocalizedString TranslateFormat(CultureInfo cultureInfo,
            string scope, string text, object[] parameters)
        {
            LogNotSet();

            return null;
        }

        public LocalizedString TranslatePluralization(CultureInfo cultureInfo,
            string scope, string text, int number)
        {
            LogNotSet();

            return null;
        }

        public LocalizedString TranslateConstant(CultureInfo cultureInfo, string scope, string text)
        {
            LogNotSet();

            return null;
        }

        public CultureInfo GetDefaultCulture()
        {
            LogNotSet();

            return null;
        }

        public string GetDefaultScope()
        {
            LogNotSet();

            return null;
        }
    }
}
