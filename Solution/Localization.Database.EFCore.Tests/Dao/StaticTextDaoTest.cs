using System.Collections.Generic;
using System.IO;
using System.Linq;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data.Impl;
using Localization.Database.EFCore.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.Database.EFCore.Tests.Dao
{
    [TestClass]
    public class StaticTextDaoTest
    {
        [TestMethod]
        public void FindByNameAndCultureAndScopeTest()
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
                
                        context.Database.ExecuteSqlCommand(sqlStr, new object[]{});
                        context.SaveChanges();
                    }
                }
            }

            using (StaticTextsContext context = new StaticTextsContext(builderOptions))
            {
                Culture culture = context.Culture.First(t => t.Id == 1);
                DictionaryScope dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                StaticTextDao staticTextDao = new StaticTextDao(context.StaticText);
                IStaticText result = staticTextDao.FindByNameAndCultureAndScope("support", culture, dictionaryScope, context.CultureHierarchy);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Format);
                Assert.AreEqual("# Podpora\r\nPodpora Vokabuláře webového: \r\n\r\n2012–2015 projekt MK ČR č. DF12P01OVV028 *Informační technologie ve službách jazykového kulturního bohatství (IT JAKUB)*  \r\n2010–2015 projekt MŠMT LINDAT-CLARIN č. LM2010013 *Vybudování a provoz českého uzlu pan-evropské infrastruktury pro výzkum*  \r\n2010–2014 projekt GA ČR č. P406/10/1140 *Výzkum historické češtiny (na základě nových materiálových bází)*  \r\n2010–2014 projekt GA ČR č. P406/10/1153 *Slovní zásoba staré češtiny a její lexikografické zpracování*  \r\n2005–2011 projekt MŠMT ČR LC 546 *Výzkumné centrum vývoje staré a střední češtiny (od praslovanských kořenů po současný stav)*  \r\n", result.Text);
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