using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Entity;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Service
{
    public class DatabaseDynamicTextService : DatabaseServiceBase, IDatabaseDynamicTextService
    {
        public DatabaseDynamicTextService(
            ILogger logger, CultureUoW cultureUoW, IConfiguration configuration
        ) : base(logger, cultureUoW, configuration)
        {
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            throw new System.NotImplementedException();
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            throw new System.NotImplementedException();
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText,
            IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            throw new System.NotImplementedException();
        }
    }
}
