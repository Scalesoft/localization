using Localization.Database.EFCore.Exception;
using Localization.Database.EFCore.Logging;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.EntityBuilder.Common
{
    public static class BuilderGuard
    {
        public static void ArgumentAlreadySet(string argumentName, object value, ILogger logger = null)
        {
            if (value != null)
            {
                ArgumentAlreadySet(argumentName, true, logger);
            }
        }

        public static void ArgumentAlreadySet(string argumentName, bool isSet, ILogger logger = null)
        {
            if (isSet)
            {
                var errorMessage = string.Format("{0} is already set.", argumentName);
                var builderException = new BuilderException(errorMessage);

                if (logger != null && logger.IsErrorEnabled())
                {
                    logger.LogError(errorMessage, builderException);
                }

                throw builderException;
            }

        }
    }
}