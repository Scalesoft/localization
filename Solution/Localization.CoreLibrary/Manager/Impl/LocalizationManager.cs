using System;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public abstract class LocalizationManager : ManagerBase
    {
        protected LocalizationManager(ILocalizationConfiguration configuration, ILogger logger = null) : base(configuration, logger)
        {
            //Should be empty.
        }

        protected LocalizedString TranslateFallback(string text)
        {
            switch (Configuration.TranslateFallbackMode)
            {
                case LocTranslateFallbackMode.Null:
                    return null;
                case LocTranslateFallbackMode.Key:
                    return new LocalizedString(text, text, true);
                case LocTranslateFallbackMode.Exception:
                    throw new TranslateException("string with text " + text + " not found");
                case LocTranslateFallbackMode.EmptyString:
                    return new LocalizedString(text, "");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
