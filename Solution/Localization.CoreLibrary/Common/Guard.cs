using System;
using Microsoft.Extensions.Logging;
using Localization.CoreLibrary.Logging;

namespace Localization.CoreLibrary.Common
{
    public static class Guard
    {
        public static void ArgumentNotNull(string argumentName, object value, ILogger logger = null)
        {
            if (value != null) return;

            var exceptionMessage = $"Value {argumentName} cannot be null.";

            var argumentNullException = new ArgumentNullException(exceptionMessage);
            if (logger != null && logger.IsErrorEnabled())
            {
                logger.LogError(exceptionMessage, argumentNullException);
            }

            throw argumentNullException;
        }

        public static void ArgumentNotNullOrEmpty(string argumentName, string value, ILogger logger = null)
        {
            ArgumentNotNull(argumentName, value, logger);

            if (value.Length != 0) return;

            var exceptionMessage = $"Value {argumentName} cannot be an empty string.";

            var argumentException = new ArgumentException(exceptionMessage);
            if (logger != null && logger.IsErrorEnabled())
            {
                logger.LogError(exceptionMessage, argumentException);
            }

            throw argumentException;
        }
    }
}