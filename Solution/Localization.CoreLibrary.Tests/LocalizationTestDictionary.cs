using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests
{
    [TestClass]
    public class LocalizationTestDictionary
    {
        private const string DefaultCulture = "cs";
        private const string SupportedCulture = "en";
        private const string TestDictionaryScope = "slovniky";
        private const string TestPartName = "popisky";

        private IAutoDictionaryManager m_dictionaryManager;

        [TestInitialize]
        public void SetUp()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo(DefaultCulture),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo(SupportedCulture),
                    new CultureInfo("es"),
                    new CultureInfo(DefaultCulture),
                },
                FirstAutoTranslateResource = LocLocalizationResource.File,
                AutoLoadResources = true
            };

            Localization.Init(localizationConfiguration);

            m_dictionaryManager = Localization.Instance();

            Localization.LibDeinit();
        }

        [TestMethod]
        public void GetDefaultDictionaryTest()
        {
            var dictionary = m_dictionaryManager.GetDictionary(LocTranslationSource.File);
            dictionary.TryGetValue("text-1-odst", out var q0);
            dictionary.TryGetValue("text-2-odst", out var q1);
            dictionary.TryGetValue("text-3-odst", out var q2);
            dictionary.TryGetValue("text-4-odst", out var q3);
            dictionary.TryGetValue("text-5-odst", out var q4);

            Assert.AreEqual("První odstavec v globálním slovníku", q0);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", q1);
            Assert.AreEqual("Třetí odstavec v globálním slovníku", q2);
            Assert.AreEqual("Čtvrtý odstavec v globálním slovníku", q3);
            Assert.AreEqual("Pátý odstavec v globálním slovníku", q4);
        }

        [TestMethod]
        public void GetManuallyDefaultDictionaryTest()
        {
            var dictionary = m_dictionaryManager.GetDictionary(LocTranslationSource.File, new CultureInfo(DefaultCulture));
            dictionary.TryGetValue("text-1-odst", out var q0);
            dictionary.TryGetValue("text-2-odst", out var q1);
            dictionary.TryGetValue("text-3-odst", out var q2);
            dictionary.TryGetValue("text-4-odst", out var q3);
            dictionary.TryGetValue("text-5-odst", out var q4);

            Assert.AreEqual("První odstavec v globálním slovníku", q0);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", q1);
            Assert.AreEqual("Třetí odstavec v globálním slovníku", q2);
            Assert.AreEqual("Čtvrtý odstavec v globálním slovníku", q3);
            Assert.AreEqual("Pátý odstavec v globálním slovníku", q4);
        }

        [TestMethod]
        public void GetManuallyScopedDictionaryTest()
        {
            var dictionary = m_dictionaryManager.GetDictionary(
                LocTranslationSource.File, new CultureInfo(DefaultCulture), TestDictionaryScope
            );
            dictionary.TryGetValue("informace-1-odst", out var q0);
            dictionary.TryGetValue("informace-2-odst", out var q1);
            dictionary.TryGetValue("informace-3-odst", out var q2);
            dictionary.TryGetValue("informace-4-odst", out var q3);
            dictionary.TryGetValue("informace-5-odst", out var q4);

            Assert.AreEqual(
                "V oddílu Slovníky Vokabuláře webového poskytujeme zájemcům o historickou češtinu informace o její slovní zásobě. Tvoří jej různorodé lexikální zdroje, které umožňují jednotné <a href=\"http://domena/Dictionaries/Dictionaries/Search\">vyhledávání</a> a <a href=\"http://domena/Dictionaries/Dictionaries/Listing\">listování</a>, tj. procházení slovníkovými zdroji „po listech“. Poučení o způsobech, jakými lze dotaz formulovat, najde uživatel v <a href=\"http://domena/Dictionaries/Dictionaries/Help\">Nápovědě</a>."
                , q0);
            Assert.AreEqual(
                "Základ oddílu tvoří tato novodobá lexikografická díla pojednávající zejména o staročeské slovní zásobě: Elektronický slovník staré češtiny (ESSČ), Malý staročeský slovník (MSS), pracovní heslář k lístkové kartotéce Staročeského slovníku (HesStčS), Slovník staročeský Jana Gebauera (GbSlov), Staročeský slovník (StčS), Slovníček staré češtiny Františka Šimka (ŠimekSlov) a Index Slovníku staročeských osobních jmen Jana Svobody (IndexSvob)."
                , q1);
            Assert.AreEqual(
                "Jsou zde však též dostupné elektronické verze historických slovníků a podobných lexikografických příruček z období 16. až 19. století. Jedná se např. o Česko-německý slovnář Jana Václava Pohla, jehož elektronickou edici vytvořil se svými kolegy prof. Tilman Berger z univerzity v Tubinkách; dále Dodavky ke slovníku Josefa Jungmanna od F. L. Čelakovského; Slovář český Jana Františka Josefa Ryvoly a Deutsch-böhmisches Wörterbuch Josefa Dobrovského, Thesaurus Linguae Bohemicae Václava Jana Rosy a další slovníky, jejichž elektronické verze vznikají v oddělení vývoje jazyka ÚJČ a které primárně poskytují transliterovaný text, doplněný v některých případech o transkripci české jazykové části.",
                q2);
            Assert.AreEqual(
                "Formou digitalizovaných obrazů doplněných o metainformace (soupis hesel a podheslí) zveřejňujeme Slovník česko-německý Josefa Jungmanna. Jedná se o fotokopie archivního exempláře, v němž sám autor označoval opravy a doplňky textu pro zamýšlené druhé vydání slovníku. Digitální kopii lze prohlížet dvěma způsoby: pomocí listování a vyhledávání v heslových slovech a podheslích, a to podle transliterované i transkribované podoby.",
                q3);
            Assert.AreEqual(
                "Informace získané vyhledáváním či listováním se liší v závislosti na zdroji, z něhož jsou čerpány – slovníky poskytnou heslovou stať, heslář ke kartotéce Staročeského slovníku pochopitelně pouze samotný výraz bez dalšího lexikografického zpracování apod. Získané údaje lze archivovat v tiskové verzi.",
                q4);
        }

        [TestMethod]
        public void GetSupportedNoScopedDictionaryTest()
        {
            var dictionary = m_dictionaryManager.GetDictionary(LocTranslationSource.File, new CultureInfo(SupportedCulture));
            dictionary.TryGetValue("text-1-odst", out var q0);
            dictionary.TryGetValue("text-2-odst", out var q1);
            dictionary.TryGetValue("text-3-odst", out var q2);
            dictionary.TryGetValue("text-4-odst", out var q3);
            dictionary.TryGetValue("text-5-odst", out var q4);

            Assert.AreEqual("The first paragraph in global dictionary", q0.Value);
            Assert.AreEqual("The second paragraph in global dictionary", q1.Value);
            Assert.AreEqual("The third paragraph in global dictionary", q2.Value);
            Assert.AreEqual("The fourth paragraph in global dictionary", q3.Value);
            Assert.AreEqual("The fifth paragraph in global dictionary", q4.Value);
        }

        [TestMethod]
        public void GetSupportedScopedDictionaryTest()
        {
            var dictionary = m_dictionaryManager.GetDictionary(
                LocTranslationSource.File, new CultureInfo(SupportedCulture), TestDictionaryScope
            );
            dictionary.TryGetValue("informace-1-odst", out var q0);

            Assert.AreEqual(
                "I had to parse JSON into key value pairs recently. The key would be the path of the JSON property. Consider the following JSON:",
                q0);
        }
    }
}
