using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseServiceFactory
    {
        IDatabaseTranslateService CreateTranslateService(ILocalizationConfiguration configuration, ILoggerFactory loggerFactory);

        IDatabaseDictionaryService CreateDictionaryService(ILocalizationConfiguration configuration, ILoggerFactory loggerFactory);

        IDatabaseDynamicTextService CreateDatabaseDynamicTextService(ILocalizationConfiguration configuration, ILoggerFactory loggerFactory);
    }
}