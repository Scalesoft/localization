using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseServiceFactory
    {
        IDatabaseTranslateService CreateTranslateService(IConfiguration configuration, ILoggerFactory loggerFactory);

        IDatabaseDictionaryService CreateDictionaryService(IConfiguration configuration, ILoggerFactory loggerFactory);
    }
}