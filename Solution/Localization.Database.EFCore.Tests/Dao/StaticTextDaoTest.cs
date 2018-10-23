using System.Collections.Generic;
using System.IO;
using System.Linq;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data.Impl;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.Database.EFCore.Tests.Dao
{
    [TestClass]
    public class StaticTextDaoTest
    {
        private DbContextOptions<StaticTextsContext> m_builderOptions;

        [TestInitialize]
        public void InitContext()
        {
            m_builderOptions = new DbContextOptionsBuilder<StaticTextsContext>()
                .UseSqlServer(Configuration.ConnectionString).Options;

            using (StaticTextsContext context = new StaticTextsContext(m_builderOptions))
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
        }

        private List<string> LookupSortedSqlFileNames()
        {
            string[] sqlFiles = Directory.GetFiles("Resources");

            List<string> result = sqlFiles.ToList();
            result.Sort();

            return result;
        }
        
        [TestMethod]
        public void FindByNameAndCultureAndScopeTest()
        {
            using (StaticTextsContext context = new StaticTextsContext(m_builderOptions))
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

        [TestMethod]
        public void FindAllByCultureAndScopeTest()
        {
            using (StaticTextsContext context = new StaticTextsContext(m_builderOptions))
            {
                Culture culture = context.Culture.First(t => t.Id == 1);
                DictionaryScope dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                StaticTextDao staticTextDao = new StaticTextDao(context.StaticText);
                StaticText[] result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                Assert.AreEqual(7, result.Length);
                Assert.AreEqual("support", result[0].Name);
            }
        }

        [TestMethod]
        public void FindAllConstantByCultureAndScopeTest()
        {
            using (StaticTextsContext context = new StaticTextsContext(m_builderOptions))
            {
                Culture culture = context.Culture.First(t => t.Id == 1);
                DictionaryScope dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                ConstantStaticTextDao staticTextDao = new ConstantStaticTextDao(context.ConstantStaticText);
                ConstantStaticText[] result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                Assert.AreEqual(1, result.Length);
                Assert.AreEqual("Pondělí", result[0].Text);
            }
        }
    }
}