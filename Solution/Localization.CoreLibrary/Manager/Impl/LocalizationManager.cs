using System;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public abstract class LocalizationManager : ManagerBase
    {
        protected LocalizationManager(LocalizationConfiguration configuration, ILogger<LocalizationManager> logger = null) : base(configuration, logger)
        {
            //Should be empty.
        }

        protected LocalizedString TranslateFallback(string text)
        {
            switch (m_configuration.TranslateFallbackMode)
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
