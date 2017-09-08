using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: InternalsVisibleTo("Localization.Database.EFCore.Tests")]
namespace Localization.Database.EFCore.Logging
{
    internal class NullLoggerFactory : ILoggerFactory
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