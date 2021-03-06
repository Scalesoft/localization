using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Model;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class NullDatabaseDynamicTextService : IDatabaseDynamicTextService
    {
        private readonly ILogger m_logger;
        private const string NotSetMessage = "Database dynamic text service is not set.";

        public NullDatabaseDynamicTextService(ILogger<NullDatabaseDynamicTextService> logger = null)
        {
            m_logger = logger;
        }

        private void LogNotSet()
        {
            if (m_logger != null && m_logger.IsInformationEnabled())
            {
                m_logger.LogInformation(NotSetMessage);
            }
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            LogNotSet();

            return null;
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            LogNotSet();

            return new List<DynamicText>();
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText,
            IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing)
        {
            LogNotSet();

            return null;
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            LogNotSet();
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            LogNotSet();
        }
    }
}
