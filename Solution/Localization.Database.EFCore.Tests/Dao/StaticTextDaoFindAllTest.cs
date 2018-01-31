using System.Collections.Generic;
using System.IO;
using System.Linq;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data.Impl;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.Database.EFCore.Tests.Dao
{
    [TestClass]
    public class StaticTextDaoFindAllTest
    {
        [TestMethod]
        public void FindAllByCultureAndScopeTest()
        {
            DbContextOptions<StaticTextsContext> builderOptions = new DbContextOptionsBuilder<StaticTextsContext>()
                .UseSqlServer(Configuration.ConnectionString).Options;

            using (StaticTextsContext context = new StaticTextsContext(builderOptions))
            {
                List<string> sqlFiles = LookupSortedSqlFileNames();
                foreach (string sqlFile in sqlFiles)
                {
                    using (var stream = new FileStream(sqlFile, FileMode.Open, FileAccess.Read))
                    using (var streamReader = new StreamReader(stream))
                    {
                        string sqlStr = streamReader.ReadToEnd();

                        context.Database.ExecuteSqlCommand(sqlStr);
                        context.SaveChanges();
                    }
                }
            }

            using (StaticTextsContext context = new StaticTextsContext(builderOptions))
            {
                Culture culture = context.Culture.First(t => t.Id == 1);
                DictionaryScope dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                StaticTextDao staticTextDao = new StaticTextDao(context.StaticText);
                StaticText[] result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                Assert.AreEqual(7, result.Length);
                Assert.AreEqual("support", result[0].Name);

            }
        }

        private List<string> LookupSortedSqlFileNames()
        {
            string[] sqlFiles = Directory.GetFiles("Resources");

            List<string> result = sqlFiles.ToList();
            result.Sort();

            return result;
        }
    }
}
