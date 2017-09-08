BEGIN TRAN

SET IDENTITY_INSERT [dbo].[BaseText] ON 

--DictionaryScope = home
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (1, 1, 2, 'support', 1, N'StaticText', N'# Podpora
Podpora Vokabuláře webového: 

2012–2015 projekt MK ČR č. DF12P01OVV028 *Informační technologie ve službách jazykového kulturního bohatství (IT JAKUB)*  
2010–2015 projekt MŠMT LINDAT-CLARIN č. LM2010013 *Vybudování a provoz českého uzlu pan-evropské infrastruktury pro výzkum*  
2010–2014 projekt GA ČR č. P406/10/1140 *Výzkum historické češtiny (na základě nových materiálových bází)*  
2010–2014 projekt GA ČR č. P406/10/1153 *Slovní zásoba staré češtiny a její lexikografické zpracování*  
2005–2011 projekt MŠMT ČR LC 546 *Výzkumné centrum vývoje staré a střední češtiny (od praslovanských kořenů po současný stav)*  
', N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (2, 1, 2, 'about', 1, N'StaticText', N'# O Vokabuláři webovém
*Vokabulář webový* jsou internetové stránky, které od listopadu 2006 postupně zpřístupňují textové, obrazové a zvukové zdroje k poznání historické češtiny. Tvůrcem a provozovatelem *Vokabuláře webového* je [oddělení vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i.](http://www.ujc.cas.cz/zakladni-informace/oddeleni/oddeleni-vyvoje-jazyka/) (dále ÚJČ). *Vokabulář webový* je autorské dílo chráněné ve smyslu aktuálního znění zákona č. 121/2000 Sb., o právu autorském, a je určen pouze k nekomerčnímu využití. Veškeré materiály poskytujeme se souhlasem nositelů autorských a reprodukčních práv.

Na stránky *Vokabuláře webového* jsme umístili různorodé zdroje. K vyhledávání informací o slovní zásobě hstorické češtiny slouží především oddíl [Slovníky](/Dictionaries), který tvoří různorodé novodobé i dobové lexikální zdroje. V oddílu [Edice](/Editions) jsou prameny zaznamenané starší češtinou prezentovány jako souvislý text společně s textově-kritickým komentářem. Souhrnně lze starší české texty prohledávat v části [Korpusy](/BohemianTextBank), [staročeský korpus](#) obsahuje texty staročeských pramenů z cca 13. až 15. století a zároveň může do určité míry nahradit dokladovou část u těch staročeských lexikografických zdrojů, které ji neobsahují. Do [staročeského korpusu](#) zahrnujeme texty z období 16. až 18. století. Díla starší české literatury poskytujeme též ve formě [audioknih](/AudioBooks).

Poučení o historické češtině najde uživatel také v oddílu [Mluvnice](/OldGrammar), který prezentuje digitalizované verze mluvnic a obdobných příruček z 16. až počátku 20. století. Mluvnice slouží nejen ke studiu dobového stavu a proměn českého jazykového systému, ale též k bádání o vývoji českého mluvnictví. V části [Odborná literatura](/ProfessionalLiterature) jsou zveřejněny digitalizované verze odborných textů, které se věnují problematice historické češtiny (*Historická mluvnice jazyka českého* Jana Gebauera).

V oddíle [Bibliografie](/Bibliographies) nabízíme vyhledávání v bibliografických záznamech odborné literatury k problematice staršího českého jazyka a literatury.

V části [Kartotéky](/CardFiles) zpřístupňujme digitalizovanou podobu dvou kartoték Jana Gebauera: kartotéky excerpce ze staročeské literatury a kartotéky pramenů k této excerpci.

V oddíle [Pomůcky](/Tools) nabízíme softwarové nástroje a pomůcky pro práci s digitalizovanými zdroji. Tyto nástroje si mohou zájemci zdarma stáhnout a používat, a to včetně zdrojových kódů.

Snažíme se, aby se uživatelům s *Vokabulářem webovým* dobře pracovalo a aby jeho prostřednictvím získávali informace rychle a spolehlivě. Ačkoliv usilujeme o bezproblémový chod, jsme si vědomi toho, že se zřejmě nevyhneme nedostatkům a obtížím, které se mohou vyskytnout. Budeme rádi, když nás uživatelé budou informovat formou [připomínek](/Home/Feedback) nejen o své zkušenosti s fungováním *Vokabuláře webového*, ale také o chybějících či nesprávných údajích a informacích, a to buď prostřednictvím odkazu *Připomínky* v jednotlivých oddílech, nebo pomocí odkazu *Připomínky* v zápatí internetových stránek.

V budoucnosti se bude *Vokabulář webový* rozrůstat o další zdroje. Časový harmonogram „naplňování“ záměrně neuvádíme – rozvoj bude postupný, závislý na mnohých okolnostech.

Přístup ke stránkám *Vokabuláře webového* je bezplatný. Pokud budete užívat nalezené informace ve svých publikacích, citujte je podle [návodu](/Home/HowToCite).

Tvorba *Vokabuláře webového* je [podporována](/Home/Support) z různých zdrojů.'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (3, 1, 2, 'copyright', 1, N'StaticText', N'# Copyright
Copyright © 2006–2015, oddělení vývoje jazyka, Ústav pro jazyk český AV ČR, v. v. i.

*Podmínky užití*  
*Vokabulář webový* je autorské dílo chráněné ve smyslu aktuálního znění zákona č. 121/2000 Sb., o právu autorském, a slouží výhradně k nekomerčnímu využití. Bez předchozí konzultace s oddělením vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i., je zakázáno rozšiřovat jakoukoliv jeho část, ať již samostatně, či v rámci jiného projektu. Při [citaci](/Home/HowToCite) *Vokabuláře webového* či jeho součástí je nutné postupovat podle obecně uznávaných citačních pravidel.'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (4, 1, 2, 'contacts', 1, N'StaticText', N'# Kontakty
*adresa:*  
oddělení vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i.  
Valentinská 1  
116 46 Praha 1  
[http://www.ujc.cas.cz/zakladni-informace/oddeleni/oddeleni-vyvoje-jazyka/](http://www.ujc.cas.cz/zakladni-informace/oddeleni/oddeleni-vyvoje-jazyka/)

*e-mail:*  
[vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz)

*telefon:*  
+420 225 391 452

*mapa:*  
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
<div style="overflow: hidden; height: 500px; width: 600px;">
    <div id="gmap_canvas" style="height: 500px; width: 600px;"></div>
    <style>
        #gmap_canvas img {
            max-width: none !important;
            background: none !important;
        }
    </style><a class="google-map-code" href="http://www.map-embed.com" id="get-map-data">http://www.map-embed.com</a>
</div>
<script type="text/javascript">
    function init_map() {
        var myOptions = {
            zoom: 18,
            center: new google.maps.LatLng(50.0874414, 14.416664099999934),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map(document.getElementById("gmap_canvas"), myOptions);
        marker = new google.maps.Marker({
            map: map,
            position: new google.maps.LatLng(50.0874414, 14.416664099999934)
        });
        infowindow = new google.maps.InfoWindow({
            content:
                "<b>oddělení vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i.</b><br/>Valentinsk&aacute; 1<br/>12800 Praha"
        });
        google.maps.event.addListener(marker, "click", function() { infowindow.open(map, marker); });
        infowindow.open(map, marker);
    }
    google.maps.event.addDomListener(window, ''load'', init_map);
</script>'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (5, 1, 2, 'links', 1, N'StaticText', N'# Odkazy
Ústav pro jazyk český AV ČR, v. v. i., Letenská 4, Praha 1, 118 51 [http://www.ujc.cas.cz](http://www.ujc.cas.cz)

Manuscriptorium, virtuální badatelské prostředí pro oblast historických fondů [http://www.manuscriptorium.com/](http://www.manuscriptorium.com/)  
Centrum medievistických studií (digitalizované edice a jiné zdroje k českým středověkým dějinám) [http://cms.flu.cas.cz/](http://cms.flu.cas.cz/)  
Virtuální archiv listin střední Evropy [http://monasterium.net/mom/home?_lang=ces](http://monasterium.net/mom/home?_lang=ces)  
Český národní korpus [http://ucnk.ff.cuni.cz](http://ucnk.ff.cuni.cz)  
Elektronická verze Příručního slovníku jazyka českého a naskenovaný lístkový archiv k PSJČ [http://psjc.ujc.cas.cz](http://psjc.ujc.cas.cz)  
Elektronická verze Slovníku spisovného jazyka českého [http://ssjc.ujc.cas.cz](http://ssjc.ujc.cas.cz)  
Lexikální databáze humanistické a barokní češtiny, dostupná po registraci z adresy [http://madla.ujc.cas.cz](http://madla.ujc.cas.cz)  
LEXIKO – webové hnízdo o novodobé české slovní zásobě a výkladových slovnících [http://lexiko.ujc.cas.cz/](http://lexiko.ujc.cas.cz/)  
Latinsko-české slovníky mistra Klareta (elektronická edice knihy Klaret a jeho družina) [http://titus.uni-frankfurt.de/texte/etcs/slav/acech/klaret/klare.htm](http://titus.uni-frankfurt.de/texte/etcs/slav/acech/klaret/klare.htm)  
Elektroniczny słownik języka polskiego XVII i XVIII wieku [http://sxvii.pl/](http://sxvii.pl/)  
Historický slovník slovenského jazyka V (R—Š), Bratislava 2000 [http://slovnik.juls.savba.sk/?d=hssjV](http://slovnik.juls.savba.sk/?d=hssjV)  
Staročeská sbírka (lístkový katalog) Ústavu pro českou literaturu AV ČR, v. v. i. [http://starocech.ucl.cas.cz](http://starocech.ucl.cas.cz)  

Moravská zemská knihovna v Brně [http://www.mzk.cz](http://www.mzk.cz)  
Národní knihovna České republiky v Praze [http://www.nkp.cz](http://www.nkp.cz)  
Vědecká knihovna v Olomouci [http://www.vkol.cz](http://www.vkol.cz)  
Knihovna Strahovského kláštera v Praze [http://www.strahovskyklaster.cz/webmagazine/page.asp?idk=282](http://www.strahovskyklaster.cz/webmagazine/page.asp?idk=282)  
Knihovna Národního muzea v Praze [http://www.nm.cz/Knihovna-NM/](http://www.nm.cz/Knihovna-NM/)  
Archiv Pražského hradu (knihovna Metropolitní kapituly u sv. Víta) [http://old.hrad.cz/castle/archiv/index.html](http://old.hrad.cz/castle/archiv/index.html)  
Městská knihovna v Praze [http://www.mlp.cz](http://www.mlp.cz)  
Knihovna Akademie věd ČR [http://www.lib.cas.cz](http://www.lib.cas.cz)  
', N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (6, 1, 2, 'howtocite', 1, N'StaticText', N'# Jak citovat
*Vokabulář webový* je médium proměnlivé. Jednak postupně zpřístupňujeme další zdroje, jednak u děl nedokončených (jako je například *Elektronický slovník staré češtiny*) informace doplňujeme a v neposlední řadě také opravujeme chyby, které se ve zdrojích zpřístupněných prostřednictvím *Vokabuláře webového* podaří najít. Také z těchto důvodů nemají publikované korpusy povahu referenčních korpusů.

Při citaci zveřejněných děl nebo celého *Vokabuláře webového* uvádějte datum citování. Pokud budete citovat konkrétní zdroj nebo heslovou stať, používejte pro detailnější určení také datum poslední změny textu. Tento údaj se u slovníků zobrazuje v závěru každé heslové stati, u ostatních děl v informacích o zdroji.

Bibliografická citace jednotlivých děl podle normy ČSN ISO 690 je dostupná <span style="background-color: yellow">pod odkazem Jak citovat</span> na stránce s detailními informacemi o jednotlivých zdrojích.

### Při citování údajů z *Vokabuláře webového* doporučujeme následující způsoby:
&nbsp;

#### Při odkazování na webové stránky jako celek:

Vokabulář webový. *Vokabulář webový* [online]. Praha: oddělení vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i., 2015 [cit. 2015-06-21]. Dostupné z: http://vokabular.ujc.cas.cz 

#### Při odkazování na konkrétní zdroj (slovník, edici, mluvnici, odbornou literaturu aj.):

BĚLIČ, Jaromír, Adolf KAMIŠ a Karel KUČERA. *Malý staročeský slovník* [online]. 1. vyd. Praha: Státní pedagogické nakladatelství, 1978, 2014-03-12, 707 s. 
[cit. 2015-06-21]. Dostupné z: http://vokabular.ujc.cas.cz/slovniky/mss 

#### Při odkazování na korpus:

Staročeský korpus. *Staročeský korpus* [on-line]. Praha: oddělení vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i., 2015-03-12 [cit. 2015-06-21]. Dostupné z: \<http://vokabular.ujc.cas.cz/banka.aspx\>. '
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (7, 1, 2, 'feedback', 1, N'StaticText', N'# Připomínky
Na této stránce nám můžete napsat své připomínky k provozu či k informacím *Vokabuláře webového*. Pro připomínky ke konkrétním částem *Vokabuláře webového* (např. slovníkům, edicím, mluvnicím, korpusu atd.) používejte laskavě *Připomínek* na stranách jednotlivých typů informačních zdrojů.

Pokud si přejete, abychom na Vaši připomínku odpověděli, uveďte tuto skutečnost v textu Vaší zprávy a vyplňte laskavě kolonku *Jméno* a *E-mail*. Vynasnažíme se, abychom na Vaši připomínku reagovali co nejdříve. Upozorňujeme, že **neřešíme domácí úkoly a další školní práce**. V těchto případech Vám může pomoci dostupná [odborná literatura](/ProfessionalLiterature) či [další zdroje](/Home/Links). Pokud má Vaše připomínka charakter dotazu, upozorňujeme, že **odpovědi na dotazy v Ústavu pro jazyk český AV ČR mohou být zpoplatněny**.

[Provozovatel](http://www.ujc.cas.cz/zakladni-informace/oddeleni/oddeleni-vyvoje-jazyka/) stránek si vyhrazuje právo nereagovat na připomínky, které jsou pro provoz *Vokabuláře webového* zcela nepřínosné či které jsou v rozporu s dobrými mravy.'
, N'Admin')

--DictionaryScope = dict
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (8, 1, 3, 'info', 1, N'StaticText', N'# Informace
V oddílu *Slovníky* Vokabuláře webového poskytujeme zájemcům o historickou češtinu informace o její slovní zásobě. Tvoří jej různorodé lexikální zdroje, které umožňují jednotné [vyhledávání](/Dictionaries/Dictionaries/Search) a [listování](/Dictionaries/Dictionaries/Listing), tj. procházení slovníkovými zdroji „po listech“. Poučení o způsobech, jakými lze dotaz formulovat, najde uživatel v [Nápovědě](/Dictionaries/Dictionaries/Help).

Základ oddílu tvoří tato novodobá lexikografická díla pojednávající zejména o staročeské slovní zásobě:
[Elektronický slovník staré češtiny (ESSČ)](/Dictionaries/Dictionaries/Listing?xmlId=%7B08BE3E56-77D0-46C1-80BB-C1346B757BE5%7D),
[Malý staročeský slovník (MSS)](#), [pracovní heslář k lístkové kartotéce Staročeského slovníku (HesStčS)](#),
[Slovník staročeský Jana Gebauera (GbSlov)](#),
[Staročeský slovník (StčS)](/Dictionaries/Dictionaries/Listing?xmlId=%7B1ADA5193-4375-4269-8222-D8BE81D597DB%7D),
[Slovníček staré češtiny Františka Šimka (ŠimekSlov)](#)
a [Index Slovníku staročeských osobních jmen Jana Svobody (IndexSvob)](#).

Jsou zde však též dostupné elektronické verze historických slovníků a podobných lexikografických příruček z období 16. až 19. století. Jedná se např. o [Česko-německý slovnář](#) Jana Václava Pohla, jehož elektronickou edici vytvořil se svými kolegy prof. Tilman Berger z univerzity v Tubinkách; dále [Dodavky ke slovníku Josefa Jungmanna](#) od F. L. Čelakovského; [Slovář český](#) Jana Františka Josefa Ryvoly a [Deutsch-böhmisches Wörterbuch](#) Josefa Dobrovského, [Thesaurus Linguae Bohemicae](#) Václava Jana Rosy a další [slovníky](#), jejichž elektronické verze vznikají v oddělení vývoje jazyka ÚJČ a které primárně poskytují transliterovaný text, doplněný v některých případech o transkripci české jazykové části.

Formou digitalizovaných obrazů doplněných o metainformace (soupis hesel a podheslí) zveřejňujeme [Slovník česko-německý](/Dictionaries/Dictionaries/Listing?xmlId=%7B7AC74E21-53E9-4E4D-A7CC-CA739A962E58%7D) Josefa Jungmanna. Jedná se o fotokopie archivního exempláře, v němž sám autor označoval opravy a doplňky textu pro zamýšlené druhé vydání slovníku. Digitální kopii lze prohlížet dvěma způsoby: pomocí listování a vyhledávání v heslových slovech a podheslích, a to podle transliterované i transkribované podoby.

Informace získané vyhledáváním či listováním se liší v závislosti na zdroji, z něhož jsou čerpány – slovníky poskytnou heslovou stať, heslář ke kartotéce Staročeského slovníku pochopitelně pouze samotný výraz bez dalšího lexikografického zpracování apod. Získané údaje lze archivovat v tiskové verzi.

Elektronický slovník staré češtiny, Staročeský slovník i heslář ke kartotéce Staročeského slovníku jsou díla vzniklá (či vznikající) na půdě oddělení vývoje jazyka ÚJČ; na vzniku Indexu staročeských osobních jmen Jana Svobody se podíleli pracovníci ÚJČ PhDr. Libuše Olivová-Nezbedová, CSc., a doc. PhDr. & RNDr. Karel Oliva, Dr. Za možnost zapojit do *Vokabuláře webového* elektronickou verzi Malého staročeského slovníku děkujeme autorům či nositelům autorských práv k tomuto dílu, jmenovitě prof. PhDr. Karlu Kučerovi, CSc., doc. PhDr. Heleně Běličové, DrSc., a prof. PhDr. Karlu Kamišovi, CSc., kteří našemu záměru laskavě vyšli vstříc. Za souhlas s uveřejněním [dodatku k Malému staročeskému slovníku](#), tj. stati Václava Křístka o staročeských pravopisných systémech, děkujeme nositelce autorských práv k uvedené práci PhDr. Marii Bělíkové. Stejně tak patří naše poděkování Středisku [Teiresiás](#), zastoupenému PhDr. Petrem Peňázem, které nám poskytlo Malý staročeský slovník v elektronické formě.

Obrazové i textové materiály jsou vystaveny pouze pro studijní a badatelské účely a nelze rozšiřovat jakoukoliv jejich část, ať již samostatně, či v rámci jiného projektu. V případě zájmu nás kontaktujte elektronicky na adrese [vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz?subject=zajem) nebo písemně: oddělení vývoje jazyka, Ústav pro jazyk český AV ČR, v. v. i., Valentinská 1, 116 46, Praha 1. Veškeré přejímané informace podléhají citačnímu úzu vědecké literatury.'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (9, 1, 3, 'help', 1, N'StaticText', N'# Nápověda', N'Admin')

--DictionaryScope = edition
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (10, 1, 4, 'info', 1, N'StaticText', N'# Informace
V tomto oddílu představujeme zájemcům elektronické edice česky psaných textů z období 13.–18. století, s přesahem do začátku 19. století v případech, kdy se jedná o opis staršího textu. Výchozím textem (pramenem) pro elektronickou edici je rukopis, prvotisk či starý tisk. Jen výjimečně používáme jako pramen novodobou edici, a to tehdy, je-li originální pramen nedostupný či obtížně dostupný nebo je-li novodobá edice jen těžko překonatelná (mj. vzhledem k jejímu rozsahu, jako je tomu např. u edice V. Kyase a kol., *Staročeská Bible drážďanská a olomoucká*).

Elektronické edice jsou jedním z výsledků vědecké práce oddělení vývoje jazyka a slouží především jako materiálová báze pro následný jazykový výzkum, proto je jazykový přístup k textu upřednostněn ku příkladu před literárními či historickými aspekty. Edice jsou dále začleňovány do textové báze budovaného *staročeského a středněčeského korpusu*. Spolu s digitálními kopiemi rovněž spoluvytvářejí virtuální badatelské prostředí *Manuscriptorium*.

Při editaci literárních památek jsou dodržovány zásady vědeckého zpřístupňování historických textů, a dále nezbytné formální aspekty, které umožňují prezentovat texty prostřednictvím internetu při zachování nezbytných náležitostí kritické edice. Ačkoliv jsou editoři oddělení vývoje jazyka vedeni snahou aplikovat vědecké i formální přístupy maximálně jednotně a uplatňují obecné ediční zásady, nelze tento jednotný přístup nadřadit obsahově-formálním specifikům jednotlivých pramenů. Každý z nich je originální a svébytné dílo, jehož zvláštnosti musí editor ve své práci plně respektovat.

Elektronické edice vznikají již po několik let a způsob značení textověkritického aparátu se postupně rozvíjí a upřesňuje. Při srovnání některých edic lze tedy dojít ke zjištění, že stejný jev je v jedné edici značen jinak než v jiné (např. ve starších edicích je formát poznámky užíván i pro zachycení marginálních přípisků, živého záhlaví atp.; v novějších edicích jsou tyto části signalizovány jiným způsobem, přímo v textu).

Pro připomínky k elektronickým edicím lze užít nabídku [Připomínky](/Editions/Editions/Feedback).

Edice jsou vystaveny pouze pro studijní a badatelské účely a nelze rozšiřovat jakoukoliv jejich část, ať již samostatně, či v rámci jiného projektu. V případě zájmu nás kontaktujte elektronicky na adrese [vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz?subject=zajem) nebo písemně: oddělení vývoje jazyka, Ústav pro jazyk český AV ČR, v. v. i., Valentinská 1, 116 46, Praha 1. Veškeré přejímané informace podléhají citačnímu úzu vědecké literatury.'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (11, 1, 4, 'help', 1, N'StaticText', N'# Nápověda', N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (12, 1, 4, 'principles', 1, N'StaticText', N'# Obecné ediční zásady
Při vytváření elektronických edic je uplatňován kritický přístup k vydávanému textu. Texty jsou transkribovány, tj. převedeny do novočeského pravopisného systému, s tím, že jsou respektovány specifické rysy soudobého jazyka. Elektronické edice vznikají na akademickém pracovišti zabývajícím se lingvistickým výzkumem, proto je kladen mimořádný důraz na interpretaci a spolehlivý záznam jazyka pramenného textu.

Transkripce textů se řídí obecně uznávanými edičními pravidly, jimiž jsou pro období staré a střední češtiny zejména texty Jiřího Daňhelky [Směrnice pro vydávání starších českých textů](#) (Husitský Tábor 8, 1985, s. 285–301), [Obecné zásady ediční a poučení o starém jazyce českém](#) (in: Výbor z české literatury od počátků po dobu Husovu. Praha, Nakladatelství Československé akademie věd 1957, s. 25–35) a [Obecné zásady ediční a poučení o češtině 15. století](#) (in: Výbor z české literatury doby husitské. Svazek první. Praha, Nakladatelství Československé akademie věd 1963, s. 31–41) a text Josefa Vintra [Zásady transkripce českých textů z barokní doby](#) (Listy filologické 121, 1998, s. 341–346). Na základě těchto pravidel vznikly interní *Pokyny pro tvorbu elektronických edic*. Tato obecná pravidla jsou přizpůsobována stavu a vlastnostem konkrétního textu. Při transkripci textu editor dbá na to, aby svou interpretací nesetřel charakteristické rysy jazyka a textu, zároveň však nezaznamenává jevy, které nemají pro interpretaci textu či jazyka význam (např. grafické zvláštnosti textu). Na základě uvážení editora jsou v některých případech tyto obecné ediční zásady doplněny o specifickou ediční poznámku vážící se ke konkrétnímu textu.

Součástí elektronických edic je textověkritický a poznámkový aparát, jehož obsah a rozsah je zcela v kompetenci jednotlivých editorů. Bez výjimek jsou v kritickém aparátu zaznamenány všechny zásahy do textu, tj. emendace textu (uvozené grafickou značkou ]). Uzná-li to editor za vhodné, může k vybraným úsekům uvádět také transliterované znění předlohy (v tomto případě následuje transliterovaná podoba za dvojtečkou :). Pravidelně jsou zaznamenávány informace týkající se poškození či fragmentárnosti předlohy, nejistoty při interpretaci textu atp. Naopak výjimečně jsou zachyceny mezitextové vztahy.

Elektronické edice neobsahují slovníček vykládající méně známá slova. K tomuto účelu slouží mj. slovníky zveřejněné ve *Vokabuláři webovém*.

# Struktura a forma elektronických edic
<span style="color:red">nutno aktualizovat dle budoucího stavu</span>

V přehledovém záznamu o elektronické edici je na prvním místě uvedeno jméno autora originálního textu (známe-li jej) a následuje název dokumentu; pokud se jedná o název nepůvodní, uzuální, je uzavřen v hranatých závorkách []. Dále uvádíme informace o rukopisu či tisku, z něhož byla edice pořízena (uložení, signatura, stránkové určení a datace). Tyto informace se zobrazují i při „listování“ textem edice v záhlaví dokumentu.

Další informace o elektronické edici poskytuje nabídka Detail edice, která je skryta pod ikonkou otazníku. Zde je dílo přiřazeno k literárnímu druhu a žánru a jsou zde uvedeny uzuální zkratky literární památky a pramene. Dále se zde nachází jméno editora elektronické edice a označení nositele osobnostních a majetkových autorských práv. V případě, že editor k textu vypracoval ještě specifickou ediční poznámku, vede k ní odkaz Ediční poznámka.

Text edice se zobrazí po kliknutí na ikonku otevřené knihy. Text je strukturován, tj. povinně je v nich zaznamenávána uzuální foliace či paginace. Ve výjimečných případech je v textu uvedena červenou barvou i foliace či paginace variantní (např. stránkování novodobé edice). Naznačena je i další struktura textu - text je členěn pomocí nadpisů a podnadpisů na nižší celky. V závislosti na charakteru předlohového textu mohou být v elektronické edici uvedena čísla veršů (u veršovaných předloh) či označení kapitol a veršů (u biblických textů). Zkratky označující kapitoly jsou uváděny v souladu s územ Staročeského slovníku, srov. Staročeský slovník. Úvodní stati, soupis pramenů a zkratek, Praha, Academia 1968, s. 119.

Kliknutím na zelenou ikonku stránky je možné zobrazit obsah dokumentu, tj. členění textu na kapitoly či podobné celky. Kliknutím na červenou ikonku stránky se tento obsah skryje.

Součástí elektronických edic je textověkritický a poznámkový aparát, který je v textech zachycen trojím způsobem: přímo v základním textu edice, jako rozvinovací poznámka pod čarou nebo jako bublinková nápověda k odpovídající značce poznámky pod čarou. Poznámkový aparát je možné skrýt kliknutím na červenou ikonku stránky s terčíkem v dolní části. Skryje se tak nejen textověkritický komentář, ale i proznačení stránkování. V textu zůstává vysvětlení, že text zapsaný kurzivou je text cizojazyčný, a dále vysvětlení užití různého typu závorek (pro přípisky různého typu, pro doplněný text).

Tučným písmem v horním indexu v hranatých závorkách jsou v edici signalizovány vnořené informace, které se po kliknutí zobrazí v dolní části obrazovky, kde jsou umístěny všechny vnořené informace ze zobrazené stránky; vybraná poznámka je graficky zvýrazněna. K označení vnořených informací je užito písmene či arabské číslice v hranatých závorkách:

* [a] – malé písmeno označuje emendace. U emendací je na prvním místě uvedeno kurzivou správné (tj. opravené) znění textu ve shodě s edicí, za grafickou značkou ] je uvedeno chybné znění pramenné předlohy
* [1] – arabská číslice označuje poznámky a komentáře nejrůznějšího druhu, které editor pokládal za důležité pro interpretaci textu. Mj. zde uvádíme transkripci popisků k obrázkům umístěným v textu, a to včetně případných emendací a dalších poznámek editora
* [A] – velké písmeno označuje živé záhlaví. V poznámkovém aparátu je umístěn samotný text živého záhlaví doplněný o případné poznámky editora, popř. emendace nebo transliterace

V elektronické edici je dále použito těchto typů závorek a dalšího grafického značení:

* závorky; při umístění kurzoru nad textem v níže uvedených závorkách se objeví tzv. bublinková nápověda, která objasní jejich funkci (viz níže) 
  - {} ve složených závorkách je zaznamenán text, který má charakter přípisku. Rozlišujeme přípisky co do umístění v textu (marginální a interlineární) a přípisky co do stáří vzhledem k základnímu textu (soudobou rukou a mladší rukou, dále textové orientátory a tištěné marginální přípisky.
  - [] v hranatých závorkách je zaznamenán text, který není součástí předlohového textu, ale který lze na základě pravděpodobnosti či jiného textu do edice doplnit; text v hranatých závorkách může být doplněn i o poznámku (značenou arabskou číslicí v horním indexu) s údajem, odkud je text doplněn. V hranatých závorkách jsou rovněž umístěna čísla a údaje určující strukturu textu (např. foliace, paginace, značka pro obrázek nebo jiný grafický prvek).
* odlišné písmo 
  - kurziva označuje text, který byl editorem interpretován jako text nečeský; dále nerozlišujeme, o jaký jazyk se jedná, avšak nejfrekventovanější je latina
  - větším písmem tmavězelené barvy jsou označeny nadpisy a podnadpisy
  - velké tučné písmeno v základním textu znamená, že v pramenném textu je (iluminovaná) iniciála
  - červené písmo je vyhrazeno pro variantní biblický překlad; zelené písmo pro adresáta textu (listu) a modré písmo pro tiskařské impresum (a informace s ním spojené)
* grafické znaky 
  - [#]– tzv. ležatý křížek v dvojitých lomených závorkách zastupuje obrázky, schémata či tabulky, které se nacházejí v předlohovém textu a které nelze z technických důvodů jednoduše převést do elektronické podoby. Zároveň se jedná o obsah, který nemá na interpretaci textu závažný dopad, a lze je proto vynechat. Znak [#] může být doplněn poznámkou (značenou arabskou číslicí v horním indexu), v níž je stručně popsána vynechaná pasáž
* bublinková nápověda 
  - při umístění kurzoru na text odlišující se podtečkováním, barvou, velikostí či řezem od základního textu (tučné, kurziva) se zobrazí bližší informace o důvodu užití odlišného písma. Bublinková nápověda je k dispozici i u textu v závorkách ({}, []) a u trojtečky (…), pokud se jedná o torzovité, doplněné či rekonstruované slovo
  - nápověda je k dispozici také u emendací a poznámek a slouží k rychlému zobrazení informace, která je uvedena v poznámkovém aparátu na konci stránky. Tato nápověda se však zobrazuje pouze v případě, že komentovaný úsek netvoří součást jiného delšího úseku, který je sám o sobě vybaven bublinkovou nápovědou (tj. nejde např. o emendační poznámku v rámci cizojazyčného textu nebo přípisku)'
  , N'Admin')

--DictionaryScope = textbank
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (13, 1, 5, 'info', 1, N'StaticText', N'# Informace
*Staročeský a středněčeský korpus* (dříve staročeská a středněčeská textová banka) vznikají v rámci textologické a ediční činnosti oddělení vývoje jazyka. Jsou budovány od roku 2006, odkdy jsou texty psané historickou češtinou, jejichž transkribované edice v oddělení vznikají, formálně upravovány rovněž s ohledem na korpusové zpřístupnění prostřednictvím *Vokabuláře webového*. Zprvu se hlavní důraz kladl na staročeské období – *staročeský korpus* je k dipozici veřejnosti již od r. 2008; od roku 2015 zpřístupňujeme též *středněčeský korpus*.

*Staročeský korpus* zahrnuje texty z období od nejstarších počátků historické češtiny přibližně do konce 15. století, *středněčeský korpus* zpřístupňuje texty z doby od 16. století do konce 18. století. Zařazené texty zřídka mírně přesahují stanovený horní limit – a to v případech, kdy lze předpokládat, že text vznikl ve starším období. Texty jsou do korpusu zařazovány výhradně v transkripci do novočeského pravopisu. Výchozím textem (pramenem) je rukopis, prvotisk či starý tisk. Jen výjimečně používáme jako pramen novodobou edici, a to tehdy, je-li originální pramen nedostupný či obtížně dostupný nebo je-li novodobá edice jen těžko překonatelná (mj. vzhledem k jejímu rozsahu, jako je tomu např. u edice V. Kyase a kol., *Staročeská Bible drážďanská a olomoucká*).

Zařazené texty prošly při transkripci podrobnou lingvistickou analýzou, proto je lze prezentovat s doprovodnými informacemi, a to alespoň v té míře, jakou webová prezentace dovolí. Tyto informace se týkají pramenného textu jako artefaktu a charakteristiky jazyka, jímž je pramen zaznamenán. Pro zveřejnění těchto informací bylo třeba vytvořit speciální korpusový manažer, jehož autorem je PhDr. Pavel Květoň, Ph.D. Přehledný návod k užívání manažeru je obsažen v aplikaci pod záložkou [Nápověda](/BohemianTextBank/BohemianTextBank/Help). Protože jsou korpusy postupně doplňovány, upravovány a opravovány, je při citaci korpusových dat nezbytné uvádět verzi korpusu, tj. datum, které je uvedeno v nabídkovém menu korpusového manažeru.

Korpusy nejsou anotované; až na výjimky neobsahují lemmatizaci ani morfologické charakteristiky.

Pro připomínky ke korpusům lze užít nabídku [Připomínky](/BohemianTextBank/BohemianTextBank/Feedback).'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (14, 1, 5, 'help', 1, N'StaticText', N'# Nápověda'
, N'Admin')

--DictionaryScope = grammar
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (15, 1, 6, 'info', 1, N'StaticText', N'# Informace
V oddílu Mluvnic *Vokabuláře webového* (dříve též *modul digitalizovaných mluvnic, MDM*) poskytujeme zájemcům digitalizované verze historických mluvnic a podobných jazykových příruček z období 16. až 19. století. Tuto část *Vokabuláře webového* jsme uvedli v užívání mj. na počest stého výročí založení *Kanceláře Slovníku jazyka českého*, která dala základy dnešnímu *Ústavu pro jazyk český*. V roce 2011 byl *MDM* spuštěn v testovacím provozu s několika mluvnicemi na ukázku, v roce 2012 jsme zahájili plný provoz. Zvýšil se nejen počet prezentovaných mluvnic, ale podstatně jsme též upravili a rozšířili informace o těchto významných památkách české jazykovědy.

Na projektu *Mluvnic* se podílejí pracovníci oddělení vývoje jazyka PhDr. Alena Černá, Ph.D., Mgr. Barbora Hanzová, Boris Lehečka, Mgr. Kateřina Voleková, Ph.D., s nimiž spolupracují zejména (bývalí) studenti bohemistiky a jiných oborů FF UK Praha Martina Černá, Hana Enderlová, Hana Gabrielová, Lucie Hrabalová, Petr Valenta, Anna Zitová a Zuzana Žďárská a další. Autorem charakteristik k mluvnicím je PhDr. Ondřej Koupil, Ph.D. Původní aplikaci naprogramoval Lukáš Kubis.

Digitalizované mluvnice je možné využívat pro výzkum českého mluvnictví i jako důležitý sekundární zdroj k poznání historické češtiny období 16. až 19. století. Zejména starší mluvnice se nezřídka dochovaly v nečetných exemplářích, které jsou uloženy v institucích v České republice a v zahraničí. Zpřístupněním jejich digitalizované podoby prostřednictvím internetu se výrazně rozšiřuje okruh jejich uživatelů. *Mluvnice* se stávají důležitou pomůckou nejen při bádání o historické češtině, ale též při výuce českého jazyka na středních, a zvláště na vysokých školách domácích i zahraničních.

Pro usnadnění základní orientace byly digitalizované mluvnice tzv. anotovány, tj. jednotlivé obrazy (většinou dvoustran) byly orientačně označeny lingvistickými termíny, o nichž se na příslušné (dvou)stránce pojednává. Tyto termíny vycházejí z novočeského mluvnického pojetí a v některých případech přesně neodpovídají stavu v historické gramatice (např. jako „hláska měkká“ označujeme pojednání o hlásce „c“, přestože v dobovém pojetí byla chápána jako hláska tvrdá atp.). Ve výjimečných případech, kdy kniha nemá charakter mluvnice, avšak přesto ji chceme zájemcům o vývoj českého mluvnictví zprostředkovat, zveřejňujeme publikaci bez anotace. Jedná se například o tyto knížky: *Hlasové o potřebě jednoty spisovného jazyka pro Čechy, Moravany a Slováky*. Praha, 1846; Josef Dobrovský, *Abhandlung über den Ursprung des Namens Tschech (Cžech), Tschechen*. Praha, Vídeň, 1782 a Josef Dobrovský, *Institutiones linguae Slavicae dialecti veteris, quae quum apud Russos, Serbos aliosque ritus graeci, tum apud Dalmatas glagolitas ritus latini Slavos in libris sacris obtinet: cum tabulis aeri incisis quatuor*. Vídeň, 1822.

V charakteristikách mluvnic se přihlíží také k tomu, jakých písem bylo v jednotlivých tiscích použito. Písma nejsou rozlišena detailně, např. podle velikostních stupňů, ale jen podle základního charakteru. Je zavedena tato grafická konvence:

* **polotučný nekurzivní řez** = švabach
* ***polotučný kurzivní řez*** = fraktura
* základní řez nekurzivní = antikva
* *kurziva (italika)* = polokurziva

Vzácněji užité typy (textura, grotesk) jsou přepisovány jedním z konvenčních přepisů a komentovány v poznámce. K písmům více v příslušných heslech Petr VOIT. *Encyklopedie knihy: starší knihtisk a příbuzné obory mezi polovinou 15. a počátkem 19. století I–II*. 2. vyd. Praha: Libri, 2008.

Digitalizované mluvnice, které zpřístupňujeme veřejnosti, pocházejí především z fondu knihovny ÚJČ. Některé knihy nemá naše knihovna k dispozici, a proto byly naskenovány z exemplářů jiných knihoven, jako *Knihovny Národního muzea* v Praze, *Moravské zemské knihovny* v Brně, *Národní knihovny České republiky* v Praze, *Strahovské knihovny* v Praze, *Vědecké knihovny v Olomouci*. *Státnímu oblastnímu archivu v Třeboni* jsme zavázáni za poskytnutí digitálních kopií a souhlasu se zveřejněním v případě tzv. Husovy *Abecedy* a traktátu *Orthographia Bohemica*. Některé knihy pocházejí ze soukromých sbírek. Děkujeme těmto institucím za laskavý souhlas se zveřejněním digitalizovaných kopií v rámci *Vokabuláře webového*. O nositeli reprodukčních práv k daným snímkům je možné se dočíst v detailním popisu příslušné knihy a také prostřednictvím vodoznaku, kterým jsou digitalizované obrazy opatřeny, např. ÚJČ = kniha pochází z fondu knihovny *Ústavu pro jazyk český AV ČR, v. v. i.*; KNM = kniha pochází z fondu *Knihovny Národního muzea* v Praze; MZK = kniha pochází z fondu *Moravské zemské knihovny* v Brně; NK ČR = kniha pochází z fondu *Národní knihovny ČR* v Praze; VKOL = kniha pochází z fondu *Vědecké knihovny v Olomouci*; Strahov = kniha pochází z fondu *Strahovské knihovny* v Praze; TŘEBOŇ = rukopis pochází z fondu *Státního oblastního archivu v Třeboni*; SOUKROMÉ = kniha pochází ze sbírky soukromé osoby.

Obrazové i textové materiály jsou vystaveny pouze pro studijní a badatelské účely a nelze je bez souhlasu oddělení vývoje jazyka publikovat v žádných dalších textech. V případě zájmu nás kontaktujte elektronicky na adrese [vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz?subject=zajem) nebo písemně: oddělení vývoje jazyka, Ústav pro jazyk český AV ČR, v. v. i., Valentinská 1, 116 46 Praha 1. Veškeré přejímané informace podléhají citačnímu úzu vědecké literatury.'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (16, 1, 6, 'help', 1, N'StaticText', N'# Nápověda
Oddíl *Mluvnice* umožňuje prohlížet knihy dvěma způsoby: prvním způsobem je tzv. listování v digitalizované mluvnici vyhledané v seznamu zveřejněných mluvnic. Potřebné informace je možné získat i vyhledáváním napříč všemi mluvnicemi; pro tento způsob byly digitalizované mluvnice tzv. anotovány, tj. jednotlivé obrazy (většinou dvoustran) byly orientačně označeny lingvistickými termíny, o nichž se na příslušné (dvou)stránce pojednává. Tyto termíny vycházejí z novočeského mluvnického pojetí a v některých případech neodpovídají přesně stavu v historické gramatice (např. jako „hláska měkká“ označujeme pojednání o hlásce „c“, přestože v dobovém pojetí byla chápána jako hláska tvrdá atp.). Snažili jsme se, aby vyhledávání bylo maximálně vstřícné k uživateli: do vyhledávání lze tedy zadávat jak termíny české (např. *jméno podstatné*), tak běžně užívané termíny internacionální (např. *substantivum*). Vyhledávat lze nejen podle termínů, nýbrž také podle jiných údajů (slov z názvu mluvnice a jména autora); tyto údaje lze ve složitém modu vyhledávání kombinovat. Výsledkem vyhledávání je seznam všech mluvnic, které odpovídají zadaným kritériím. Každá mluvnice se zobrazuje vždy ve zvláštním panelu, přičemž strany proznačené požadovanými lingvistickými termíny se zobrazují zvýrazněné. Pod aktuálně zobrazenou (dvou)stranou mluvnice je uveden seznam všech lingvistických termínů, které jsou ke (dvou)straně přiřazeny.'
, N'Admin')

--DictionaryScope = professional
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (17, 1, 7, 'info', 1, N'StaticText', N'# Informace
V oddílu *Odborná literatura* (dále též *OL*) poskytujeme uživatelům digitalizované verze odborných textů, které se věnují problematice historické češtiny.

V současné době obsahuje *OL* kompletní Gebauerovu *Historickou mluvnici jazyka českého*. Jedná se celkem o čtyři svazky: *Historickou mluvnici jazyka českého*, Díl I, *Hláskosloví*. Praha, ČSAV 1963, 2., doplněné vydání, *Historickou mluvnici jazyka českého*, Díl III, *Tvarosloví. I. Skloňování*. Praha, ČSAV 1960, 2., doplněné vydání, *Historickou mluvnici jazyka českého*, Díl III, *Tvarosloví. II. Časování*. Praha, ČSAV 1958, 2., doplněné vydání a *Historickou mluvnici jazyka českého*, Díl IV, *Skladba*. Praha, Česká akademie věd a umění 1929.

Na elektronické verzi Gebauerovy mluvnice se pod vedením Mgr. Michala L. Hořejšího podíleli studenti bohemistiky FF UK v Praze a další spolupracovníci – Vít Černý, Jan Dušek, Tereza Hejdová, Ivo Jelínek, Pavlína Kuderová, Lucie Marková, Jana Markvartová, Milena Mráčková, Andrea Svobodová, Tereza Tomášková, Petr Valenta a Anna Vrbová.

Jednotlivé knihy Gebauerovy mluvnice jsou uloženy dvojím způsobem – jedná se a) o fotokopie originální knižní podoby a b) o přepsaný text. Obě verze jsou propojené s aktivním „obsahem“ a „rejstříkem“ a lze je zobrazovat paralelně (vedle sebe).

Kromě vyhledávání na základě obsahu a rejstříku je možné prohlížet text také prostým listováním, a to ve fotokopiích i přepsaném textu. Podrobné informace o možnostech užívání mluvnice jsou k dispozici v [Nápovědě](/OldGrammar/OldGrammar/Help).

Obrazové i textové materiály jsou vystaveny pouze pro studijní a badatelské účely a nelze rozšiřovat jakoukoliv jejich část, ať již samostatně, či v rámci jiného projektu. V případě zájmu nás kontaktujte elektronicky na adrese [vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz?subject=zajem) nebo písemně: oddělení vývoje jazyka, Ústav pro jazyk český AV ČR, v. v. i., Valentinská 1, 116 46, Praha 1. Veškeré přejímané informace podléhají citačnímu úzu vědecké literatury.'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (18, 1, 7, 'help', 1, N'StaticText', N'# Nápověda'
, N'Admin')

--DictionaryScope = bibliographies
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (19, 1, 8, 'info', 1, N'StaticText', N'# Informace
Zde budou informace k Bibliografickemu modulu'
, N'Admin')

--DictionaryScope = cardfiles
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (20, 1, 9, 'info', 1, N'StaticText', N'# Informace
V části *Kartotéky* zpřístupňujme digitalizovanou podobu dvou kartoték Jana Gebauera: kartotéky excerpce ze staročeské literatury a kartotéky pramenů k této excerpci.

Tyto kartotéky budoval Jan Gebauer (8. 10. 1838 – 25. 5. 1907) průběžně po celou dobu svého bádání na poli historického vývoje češtiny. Využíval ji nejprve při práci na historické mluvnici češtiny, ale už od 70. let 19. století si pořizoval výpisky s cílem sestavit na jejich základě slovník staré češtiny. Podrobně excerpoval památky pocházející ze 14. století, zvláště z jeho první poloviny; v textech z 15. století, popř. i v mladších se zaměřoval jen na ta slova a jazykové jevy, které pokládal za důležité.

Výpisky pořizoval Jan Gebauer (částečně za pomoci svých žáků a spolupracovníků) na kartotéční lístky (většinou o rozměrech 17 × 10,5 cm). Na četných lístcích jsou dodatečné přípisky a komentáře, dnes již málo zřetelné. Poměrně často je na jednom lístku uvedeno několik citátů, často z různých textů.

Kartotéka obsahuje lístky dokládající slova *netbalivý – Žitomíř*, protože tato část abecedy nebyla v Gebauerově slovníku zpracována. Lístky z abecedy ve slovníku obsažené Gebauer dále neuchovával. Kdy a kde se ztratily lístky z konce abecedy (konec písmene *ž*), není známo.

Po Gebauerově smrti v roce 1907 užíval kartotéku Emil Smetánka, jeho nástupce na univerzitě, a na jejím základě vydal dva další sešity staročeského slovníku. Poté byla kartotéka uložena v *Kanceláři Slovníku jazyka českého*. Po vzniku oddělení pro studium vývoje jazyka v Ústavu pro jazyk český roku 1948 se kartotéka stala základem jeho lexikálních sbírek a byla zprvu doplňována o nově pořizovanou excerpci; proto se u slov začínajících písmenem n nacházejí některá excerpta zcela jiného vzhledu. Brzy však byly tyto výpisky řazeny do samostatné kartotéky a Gebauerova excerpce byla pietně ponechána v původním stavu.

Kartotéka J. Gebauera dnes obsahuje na 80 tisíc lístků. Její nedílnou součástí je i soubor lístků s Gebauerovými záznamy o excerpovaných a zkoumaných textech. Tyto lístky jsou řazeny abecedně podle zkratky, pod níž je příslušná památka citována v jeho historické mluvnici a slovníku.

Digitalizovaná podoba kartotéky se základní anotací byla pořízena v letech 2005–2006 s úmyslem zpřístupnit tuto kartotéku veřejnosti prostřednictvím internetu v roce stého výročí úmrtí J. Gebauera. Kartotéku digitalizovala firma Imaging Systems, spol. s r. o. Lístky byly označeny údaji, které Jan Gebauer či jeho spolupracovníci zapsali v záhlaví lístků; na této anotaci se podílely zejména tehdejší studentky Filozofické fakulty Univerzity Karlovy Tereza Honková a Eva Vachková. Užité podoby slov v záhlaví záměrně nejsou sjednocovány časově ani lokálně, jsou zde zachyceny jak podoby staročeské, tak i pozdější. Některé lístky pouze odkazují na formální variantu slova uvedenou na jiném lístku. V jiných případech je v souladu s údaji na lístku v elektronickém záhlaví uvedeno více výrazů či je výraz doplněn o otazník. Tato nejednotnost anotace odráží pracovní charakter excerpce, ovšem za cenu obtížnějšího prohledávání zveřejňovaného materiálu, při němž musí uživatel zvažovat vhodný způsob formulace dotazu (zadávání různých vývojových podob slov, užívání zástupných znaků atp.). Součástí zpřístupněného materiálu je též databáze zkratek, jež obsahuje především zkratky excerpovaných památek – tyto lístky byly (pokud možno) anotovány v souladu se zkratkami otištěnými v Gebauerově Slovníku staročeském. – Lístky bez jakéhokoli údaje v záhlaví, které jsou v kartotéce uloženy odděleně od ostatních, do databáze nezařazujeme.

Za vytvoření anotovacího a vyhledávacího programu *Bára*, pomocí něhož jsou lístky zveřejněny, děkujeme Mgr. Miroslavu Spoustovi.

Pro obecné připomínky ke zveřejnění Gebauerovy excerpce je možné využít nabídku [Připomínky](/CardFiles/CardFiles/Feedback). Připomínky k jednotlivým lístkům lze zaslat po kliknutí na odkaz *Připomínka* ve spodní části jednotlivých záznamů. <span style="color: red">– poslední větu aktualizovat dle budoucího stavu</span>'
, N'Admin')

--DictionaryScope = audio
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (21, 1, 10, 'info', 1, N'StaticText', N'# Informace
V této části *Vokabuláře webového* poskytujeme zájemcům zvukové nahrávky starších českých děl. V současnosti představujeme dvě díla – *Výbor ze staročeské prózy* a *Frantovy práva* z roku 1518.

Uživatel si může nahrávky stáhnout do vlastního zařízení nebo poslechnout přímo na internetové stránce. Záznamy mohou sloužit jako pomůcka pro výuku starší češtiny a literatury na středních školách či jako vhodné médium pro zpřístupnění starších českých děl zájemcům z řad zrakově hendikepovaných. V neposlední řadě jsou však audioknihy určeny také běžnému zájemci, kterému je auditivní formát z jakéhokoliv důvodu blízký.

Nahrávky vznikly v oddělení vývoje jazyka za vedení Mgr. Michala L. Hořejšího a Mgr. Marka Janosika-Bielského. Umělecké texty načetli Blanka Hejtmánková, Jindřich Kout a Petr Gojda. Podklad pro audioverze tvořily edice vytvořené pracovníky oddělení vývoje jazyka Ústavu pro jazyk český AV ČR, v. v. i.

Nahrávky jsou vystaveny pouze pro nekomerční využití, pro studijní a badatelské účely a nelze rozšiřovat jakoukoliv jejich část, ať již samostatně, či v rámci jiného projektu. V případě zájmu nás kontaktujte elektronicky na adrese [vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz?subject=zajem) nebo písemně: oddělení vývoje jazyka, Ústav pro jazyk český AV ČR, v. v. i., Valentinská 1, 116 46, Praha 1. Veškeré přejímané informace podléhají citačnímu úzu vědecké literatury.'
, N'Admin')

--DictionaryScope = tools
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (22, 1, 11, 'info', 1, N'StaticText', N'# Informace
V této části *Vokabuláře webového* najdete počítačové nástroje a další pomůcky určené pro badatele, které vznikly v oddělení vývoje jazyka Ústavu pro jazyk český. Obvykle jsou k dispozici zdarma, a to včetně zdrojových kódů a ukázkových dat. Vedle základních informací o účelu a fungování jednotlivých nástrojů uvádíme také detailnější nápovědu pod záložkou *Nápověda* a text licence, pod níž je daný nástroj k dispozici.

Zároveň se snažíme upozornit na další počítačové technologie, nástroje, pomůcky a programy, které při badatelské práci využíváme, a to formou odkazu a stručné anotace.

V případě jakýchkoli dotazů můžete využít kontaktní formulář [Připomínek](/Tools/Tools/Feedback) nebo se obrátit na správce stránky prostřednictvím e-mailové adresy [vokabular@ujc.cas.cz](mailto:vokabular@ujc.cas.cz).'
, N'Admin')

INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (23, 1, 11, 'list', 1, N'StaticText', N'# Seznam'
, N'Admin')

INSERT INTO [dbo].[DatabaseVersion]
	(DatabaseVersion)
VALUES
	('003' )

--ROLLBACK
COMMIT