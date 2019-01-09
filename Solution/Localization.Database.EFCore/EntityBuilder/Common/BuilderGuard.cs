using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Database.EFCore.Exception;
using Scalesoft.Localization.Database.EFCore.Logging;

namespace Scalesoft.Localization.Database.EFCore.EntityBuilder.Common
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