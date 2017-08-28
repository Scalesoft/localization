using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class NullDatabaseLocalization : ILocalizationManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private const string NotSetMessage = "Database localization manager is not set.";

        private LocalizedString LogAndThrowError()
        {
            Logger.LogError(NotSetMessage);
            throw new DatabaseLocalizationManagerException(NotSetMessage);
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return LogAndThrowError();
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null,
            string scope = null)
        {
            return LogAndThrowError();
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null,
            string scope = null)
        {
            return LogAndThrowError();
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            return LogAndThrowError();
        }

        public CultureInfo DefaultCulture()
        {
            LogAndThrowError();
            return null;
        }
    }
}