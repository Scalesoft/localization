using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests
{
    [TestClass]
    public class LocalizationTestDictionary
    {
        private string defaultCulture = @"cs";
        private string supportedCulture = @"en";
        private string testDictionaryScope = @"slovniky";
        private string testPartName = @"popisky";


        [TestInitialize]
        public void SetUp()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"local";
            configuration.DefaultCulture = defaultCulture;
            configuration.SupportedCultures = new List<string> { supportedCulture, "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.LibInit(localizationConfiguration);
           
        }

        [TestCleanup]
        public void SetDown()
        {
            Localization.LibDeinit();
        }


        [TestMethod]
        public void GetDefaultDictionaryTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionary();
            List<LocalizedString> lStringList = dictionary.ToList();
            Assert.AreEqual("První odstavec v globálním slovníku", lStringList[0]);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", lStringList[1]);
            Assert.AreEqual("Třetí odstavec v globálním slovníku", lStringList[2]);
            Assert.AreEqual("Čtvrtý odstavec v globálním slovníku", lStringList[3]);
            Assert.AreEqual("Pátý odstavec v globálním slovníku", lStringList[4]);
        }

        [TestMethod]
        public void GetManuallyDefaultDictionaryTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionary(new CultureInfo(defaultCulture));
            List<LocalizedString> lStringList = dictionary.ToList();
            Assert.AreEqual("První odstavec v globálním slovníku", lStringList[0]);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", lStringList[1]);
            Assert.AreEqual("Třetí odstavec v globálním slovníku", lStringList[2]);
            Assert.AreEqual("Čtvrtý odstavec v globálním slovníku", lStringList[3]);
            Assert.AreEqual("Pátý odstavec v globálním slovníku", lStringList[4]);
        }

        [TestMethod]
        public void GetManuallyScopedDictionaryTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionary(new CultureInfo(defaultCulture), testDictionaryScope);
            List<LocalizedString> lStringList = dictionary.ToList();
            Assert.AreEqual("V oddílu Slovníky Vokabuláře webového poskytujeme zájemcům o historickou češtinu informace o její slovní zásobě. Tvoří jej různorodé lexikální zdroje, které umožňují jednotné <a href=\"http://censeo2.felk.cvut.cz/Dictionaries/Dictionaries/Search\">vyhledávání</a> a <a href=\"http://censeo2.felk.cvut.cz/Dictionaries/Dictionaries/Listing\">listování</a>, tj. procházení slovníkovými zdroji „po listech“. Poučení o způsobech, jakými lze dotaz formulovat, najde uživatel v <a href=\"http://censeo2.felk.cvut.cz/Dictionaries/Dictionaries/Help\">Nápovědě</a>."
                , lStringList[0]);
            Assert.AreEqual("Základ oddílu tvoří tato novodobá lexikografická díla pojednávající zejména o staročeské slovní zásobě: Elektronický slovník staré češtiny (ESSČ), Malý staročeský slovník (MSS), pracovní heslář k lístkové kartotéce Staročeského slovníku (HesStčS), Slovník staročeský Jana Gebauera (GbSlov), Staročeský slovník (StčS), Slovníček staré češtiny Františka Šimka (ŠimekSlov) a Index Slovníku staročeských osobních jmen Jana Svobody (IndexSvob)."
                , lStringList[1]);
            Assert.AreEqual("Jsou zde však též dostupné elektronické verze historických slovníků a podobných lexikografických příruček z období 16. až 19. století. Jedná se např. o Česko-německý slovnář Jana Václava Pohla, jehož elektronickou edici vytvořil se svými kolegy prof. Tilman Berger z univerzity v Tubinkách; dále Dodavky ke slovníku Josefa Jungmanna od F. L. Čelakovského; Slovář český Jana Františka Josefa Ryvoly a Deutsch-böhmisches Wörterbuch Josefa Dobrovského, Thesaurus Linguae Bohemicae Václava Jana Rosy a další slovníky, jejichž elektronické verze vznikají v oddělení vývoje jazyka ÚJČ a které primárně poskytují transliterovaný text, doplněný v některých případech o transkripci české jazykové části.",
                 lStringList[2]);
            Assert.AreEqual("Formou digitalizovaných obrazů doplněných o metainformace (soupis hesel a podheslí) zveřejňujeme Slovník česko-německý Josefa Jungmanna. Jedná se o fotokopie archivního exempláře, v němž sám autor označoval opravy a doplňky textu pro zamýšlené druhé vydání slovníku. Digitální kopii lze prohlížet dvěma způsoby: pomocí listování a vyhledávání v heslových slovech a podheslích, a to podle transliterované i transkribované podoby.",
                 lStringList[3]);
            Assert.AreEqual("Informace získané vyhledáváním či listováním se liší v závislosti na zdroji, z něhož jsou čerpány – slovníky poskytnou heslovou stať, heslář ke kartotéce Staročeského slovníku pochopitelně pouze samotný výraz bez dalšího lexikografického zpracování apod. Získané údaje lze archivovat v tiskové verzi.",
                 lStringList[4]);



        }

        [TestMethod]
        public void GetSupportedNoScopedDictionaryTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionary(new CultureInfo(supportedCulture));
            List<LocalizedString> lStringList = dictionary.ToList();
            Assert.AreEqual("The first paragraph in global dictionary", lStringList[0]);
            Assert.AreEqual("The second paragraph in global dictionary", lStringList[1]);
            Assert.AreEqual("The third paragraph in global dictionary", lStringList[2]);
            Assert.AreEqual("The fourth paragraph in global dictionary", lStringList[3]);
            Assert.AreEqual("The fifth paragraph in global dictionary", lStringList[4]);
        }

        [TestMethod]
        public void GetSupportedScopedDictionaryTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionary(new CultureInfo(supportedCulture), testDictionaryScope);
            List<LocalizedString> lStringList = dictionary.ToList();

            Assert.AreEqual("I had to parse JSON into key value pairs recently. The key would be the path of the JSON property. Consider the following JSON:", lStringList[0]);
        }

        [TestMethod]
        public void GetDefaultNoScopedDictionaryPartTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionaryPart(testPartName);
            List<LocalizedString> lStringList = dictionary.ToList();

            Assert.AreEqual("Potvrdit", lStringList[0]);
        }

        [TestMethod]
        public void GetManuallyDefaultNoScopedDictionaryPartTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionaryPart(testPartName, new CultureInfo(defaultCulture));
            List<LocalizedString> lStringList = dictionary.ToList();

            Assert.AreEqual("Potvrdit", lStringList[0]);
        }

        [TestMethod]
        public void GetManuallyDefaultScopedDictionaryPartTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionaryPart(testPartName, new CultureInfo(defaultCulture), testDictionaryScope);
            List<LocalizedString> lStringList = dictionary.ToList();

            Assert.AreEqual("Žádné výsledky k zobrazení.", lStringList[0]);
        }


        [TestMethod]
        public void GetSupportedNoScopedDictionaryPartTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionaryPart(testPartName, new CultureInfo(supportedCulture));
            List<LocalizedString> lStringList = dictionary.ToList();

            Assert.AreEqual("Confirm", lStringList[0]);
        }

        [TestMethod]
        public void GetSupportedScopedDictionaryPartTest()
        {
            IEnumerable<LocalizedString> dictionary = Localization.Dictionary.GetDictionaryPart(testPartName, new CultureInfo(supportedCulture), testDictionaryScope);
            List<LocalizedString> lStringList = dictionary.ToList();

            Assert.AreEqual("No results to show.", lStringList[0]);
        }


    }
}