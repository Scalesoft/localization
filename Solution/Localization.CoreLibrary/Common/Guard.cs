using System;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Common
{
    public static class Guard
    {


        public static void ArgumentNull(string argumentName, object value, ILogger logger = null)
        {
            if (value == null)
            {
                ArgumentNullException argumentException = new ArgumentNullException();

                string errorMessage = string.Format("{0} is null.", argumentName);
                logger.LogError(errorMessage, argumentException);

                throw argumentException;
            }
        }

    }
}