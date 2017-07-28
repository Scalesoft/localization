using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Localization.CoreLibrary.Logging
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {
            //Do nothing           
        }

        public ILogger CreateLogger(string categoryName)
        {
            return NullLogger.Instance;
        }

        public void AddProvider(ILoggerProvider provider)
        {
            //Do nothing           
        }
    }
}