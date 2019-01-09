using System.Collections.Generic;
using System.IO;
using System.Linq;
using Localization.Database.Abstractions.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Database.EFCore.Dao.Impl;
using Scalesoft.Localization.Database.EFCore.Data.Impl;

namespace Scalesoft.Localization.Database.EFCore.Tests.Dao
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

            using (var context = new StaticTextsContext(m_builderOptions))
            {
                var sqlFiles = LookupSortedSqlFileNames();
                foreach (var sqlFile in sqlFiles)
                {
                    using (var stream = new FileStream(sqlFile, FileMode.Open, FileAccess.Read))
                    using (var streamReader = new StreamReader(stream))
                    {
                        var sqlStr = streamReader.ReadToEnd();

                        context.Database.ExecuteSqlCommand(sqlStr);
                        context.SaveChanges();
                    }
                }
            }
        }

        private List<string> LookupSortedSqlFileNames()
        {
            var sqlFiles = Directory.GetFiles("Resources");

            var result = sqlFiles.ToList();
            result.Sort();

            return result;
        }

        [TestMethod]
        public void FindByNameAndCultureAndScopeTest()
        {
            using (var context = new StaticTextsContext(m_builderOptions))
            {
                var culture = context.Culture.First(t => t.Id == 1);
                var dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                var staticTextDao = new StaticTextDao(context.StaticText);
                IStaticText result = staticTextDao.FindByNameAndCultureAndScope("support", culture, dictionaryScope,
                    context.CultureHierarchy);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Format);
                Assert.AreEqual(
                    "# Podpora\r\nPodpora Vokabuláře webového: \r\n\r\n2012–2015 projekt MK ČR č. DF12P01OVV028 *Informační technologie ve službách jazykového kulturního bohatství (IT JAKUB)*  \r\n2010–2015 projekt MŠMT LINDAT-CLARIN č. LM2010013 *Vybudování a provoz českého uzlu pan-evropské infrastruktury pro výzkum*  \r\n2010–2014 projekt GA ČR č. P406/10/1140 *Výzkum historické češtiny (na základě nových materiálových bází)*  \r\n2010–2014 projekt GA ČR č. P406/10/1153 *Slovní zásoba staré češtiny a její lexikografické zpracování*  \r\n2005–2011 projekt MŠMT ČR LC 546 *Výzkumné centrum vývoje staré a střední češtiny (od praslovanských kořenů po současný stav)*  \r\n",
                    result.Text);
            }
        }

        [TestMethod]
        public void FindAllByCultureAndScopeTest()
        {
            using (var context = new StaticTextsContext(m_builderOptions))
            {
                var culture = context.Culture.First(t => t.Id == 1);
                var dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                var staticTextDao = new StaticTextDao(context.StaticText);
                var result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                Assert.AreEqual(7, result.Length);
                Assert.AreEqual("support", result[0].Name);
            }
        }

        [TestMethod]
        public void FindAllConstantByCultureAndScopeTest()
        {
            using (var context = new StaticTextsContext(m_builderOptions))
            {
                var culture = context.Culture.First(t => t.Id == 1);
                var dictionaryScope = context.DictionaryScope.First(t => t.Id == 2);


                var staticTextDao = new ConstantStaticTextDao(context.ConstantStaticText);
                var result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                Assert.AreEqual(1, result.Length);
                Assert.AreEqual("Pondělí", result[0].Text);
            }
        }
    }
}
