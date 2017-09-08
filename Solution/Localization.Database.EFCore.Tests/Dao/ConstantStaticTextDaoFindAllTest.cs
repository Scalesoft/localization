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
    public class ConstantStaticTextDaoFindAllTest
    {
        [TestMethod]
        public void FindAllByCultureAndScopeTest()
        {
            DbContextOptions<StaticTextsContext> builderOptions = new DbContextOptionsBuilder<StaticTextsContext>()
                .UseSqlServer(@"Server=ENUMERATIO;Database=ITJakubWebDBLocalization;Trusted_Connection=True;").Options;

            using (StaticTextsContext context = new StaticTextsContext(builderOptions))
            {
                List<string> sqlFiles = LookupSortedSqlFileNames();
                foreach (string sqlFile in sqlFiles)
                {
                    using (Stream stream = new FileStream(sqlFile, FileMode.Open))
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        string sqlStr = streamReader.ReadToEnd();

                        context.Database.ExecuteSqlCommand(sqlStr, new object[] { });
                        context.SaveChanges();
                    }
                }
            }

            using (StaticTextsContext context = new StaticTextsContext(builderOptions))
            {
                Culture culture = context.Culture.First(t => t.Id == 1);
                DictionaryScope dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                ConstantStaticTextDao staticTextDao = new ConstantStaticTextDao(context.ConstantStaticText);
                ConstantStaticText[] result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                Assert.AreEqual(1, result.Length);
                Assert.AreEqual("Pondělí", result[0].Text);
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