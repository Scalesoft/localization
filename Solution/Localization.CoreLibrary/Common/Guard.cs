using System;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Common
{
    public static class Guard
    {
        public static void ArgumentNotNull(string argumentName, object value, ILogger logger = null)
        {
            if (value == null)
            {
                string exceptionMessage = $"Value {argumentName} cannot be null.";

                ArgumentNullException argumentNullException = new ArgumentNullException(exceptionMessage);
                if (logger != null && logger.IsErrorEnabled())
                {
                    logger.LogError(exceptionMessage, argumentNullException);
                }

                throw argumentNullException;
            }
        }
        public static void ArgumentNotNullOrEmpty(string argumentName, string value, ILogger logger = null)
        {
            ArgumentNotNull(argumentName, value, logger);
            if (value.Length == 0)
            {
                string exceptionMessage = $"Value {argumentName} cannot be an empty string.";

                ArgumentException argumentException = new ArgumentException(exceptionMessage);
                if (logger != null && logger.IsErrorEnabled())
                {
                    logger.LogError(exceptionMessage, argumentException);
                }

                throw argumentException;
            }
        }

    }
}