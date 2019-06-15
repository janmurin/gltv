using System;
using System.Linq;
using GLTV.Data;
using GLTV.Models.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GLTV.Models
{
    public static class SeedData
    {
        public static void InitializeInzeraty(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Inzerat.Any())
                {
                    return; // DB has been seeded
                }
                throw new Exception("empty database");
//                context.Inzerat.AddRange(
//                    new Inzerat()
//                    {
//                        ID = 1,
//                        Author = "Peter Lukas - Cafe Reality s.r.o.",
//                        Category = "Byty",
//                        DateInserted = DateTime.Now,
//                        Description = "Na prenájom kompletne zrekonštruovaný 3 izbový byt o veľkosti 90 m2, Bratislava, Staré Mesto, Šancová ulica, Širšie centrum Je po kompletnej rekonštrukcii. Nachádza sa v tehlovom bytovom dome na 4. poschodí 6 poschodového bytového domu. Byt sa prenajíma zariadený a vybavený: s vybavenou kuchyňou so spotrebičmi: zabudovanou mikrovlnkou a chladničkou, umývačkou riadu, plynovou varnou doskou, teplovzdušnou rúrou a práčkourohová. Obývačka: rohová sedačka, LCD TV, skrinky, manželská posteľ, vstavané skrine v jednej spálni a v druhej spálni vstavaná skriňa. Byt je vybavený ako je na fotkách. Pozostáva: zo vstupnej chodby z obývacej izby, 2x spálňa a samostatnou kuchyňou. Z kúpelne so sprchovým kútom a samostatného WC. Byt má bezpečnostné vchodové dvere s alarmom, plastové okná, kompletne zrekonštruované elektrické rozvody, stierky, interiérové dvere. Dve izby sú otočené do tichého dvora a jedna izba s kuchyňou do ulice. V byte je zaklenná loggia otočená do ulice. Parkovanie je možnosť v uzatvorenom dvore. Cena 550,- EUR/mes. je uvedená bez energií, energie sú + 300,- EUR/mes (internet, TV, elektrika, správca, smeti, parkovanie) spolu 850,- EUR/mes.. Depozit je vo výške mesačného nájmu s energiami. Provízia je vo výške 400,- EUR. Byt je voľný IHNEĎ k nasťahovaniu. V blízkosti sa na nachádza: trhovisko na Žilinskej, potraviny otvorené do 23 hod., obchody, reštaurácie, pivárne, kompletné služby, MHD s dobrou dostupnosťou do všetkých častí mesta, tržnica, obchodné centrum Centrál V blízkosti ulíc: Palárikova, Karpatská, Žilinská, Björnsonova, Povraznícka, Záhrebská, Tabakova, Mýtna, Anenská, Benediktiho, Belopotockého, Vazovova, Karpatská, Palárikova, Jelenia, Murgašova, Železničiarska Poschodie: 3 Počet poschodí: 6 Počet izieb: 3 Dodatok k cene: Cena je uvedená bez energií, +energie Obytná plocha: 90m? Celková rozloha: 90m? Úžitková plocha: 90m? Vlastníctvo: osobné Stav: úplne prerobený Odvoz odpadu: áno - separovaný Voda: verejný vodovod El. napätie: 230V Plyn: nie Kanalizácia: áno Internet: telefónna linka Kúpeľňa: sprchovací kút Vykurovanie: spoločné Lódžia: áno Pivnica: nie Výťah: áno Parkovanie: verejné Postavené z: tehla Zariadenie: čiastočne Strecha: po rekonštrukcii Káblová televízia: áno Energetický certifikát: B Ponúkame na prenájom zariadený 3 izbový byt, v trojpodlažnej vile na Kubániho ulici. Dom je oplotený obkolesený záhradou, o ktorú sa stará záhradník. Byt sa nachádza na zvýšenom prízemí vily. Je orientovaný na 2 strany – spálňa, detská izba sú na východ a kuchyňa, obývačkou na západ. Rozloha bytu j ... Dore den! Potrebujem byt 2 izbe v.ul.stara mesto albo nove mesto . Surne dnes Cena:500€ do 550€ Zariadenie nemusi bit aj može.pre 3ludi.. Ponúkam na dlhodobý prenájom priestranný, slnečný 3-izbový byt v Starom meste, voľný ihneď. Dispozícia bytu je kuchyňa, 2 kúpeľne (sprcha s umývadlom a Wc, umývadlo, práčka), veľká chodba, obývačka s balkónom a 2 spálne-nepriechodné, každá cca 20 m2, jedna z nich aj s väčším balkónom. Byt je po ... Ponúkame na prenájom kompletne zariadený 3 i byt priamo v centre na Jedlíkovej ul. neďaleko Kamenného námestia, vhodný aj pre rodinu prípadne aj slušných študentov. Byt je veľkometrážny, 100 m2, balkón, 2.NP, má 3 samostatné veľké izby, chodbu, samostatnú kuchyňu s umývačkou riadu, balkón, kúpeňa j ... Garzónka na 1. poschodí tehlového domu bez výťahu. Úplne nová rekonštrukcia, prvý nájomník. Kuchynská linka s drezom, samostatne stojaci sporák so sklokeramickou varnou doskou, digestor, chladnička s mrazničkou, klimatizácia, šatníková skriňa, policový regál, dlažba, v kúpeľni vaňa. Možnosť úplné ... Ponúkame do prenájmu 2izb.byt na Záhradníckej ul. (pri Justičnom paláci a Lekárskej fakulte UK), 1.p./5.p., 50m2, nový, po kompletnej rekonštrukcii, kompletne novo zariadený aj so spotrebičmi, situovaný do tichého dvora. Vhodný pre max 2 osoby, vhodný aj pre zahraničných záujemcov, aj študentov Leká ...",
//                        Email = "[]",
//                        Location = "Ostatné",
//                        Phone = "+421907055744",
//                        Portal = "http://reality.bazos.sk",
//                        Price = "550 €",
//                        Title = "Na prenájom 3 izbový byt, Bratislava, Staré Mesto, Šancová u",
//                        Type = "Prenájom",
//                        Url = "http://reality.bazos.sk/inzerat/99367532/Na-prenajom-3-izbovy-byt-Bratislava-Stare-Mesto-Sancova-ulica.php"
//                    },
//new Inzerat()
//{
//    ID = 2,
//    Author = "Ing.Božiková",
//    Category = "Byty",
//    DateInserted = DateTime.Now,
//    Description = "Predáme 3-izbový byt na KLokočine -Novomeského ul., OV, 78m2, čiastočná rekonštrukcia- plastové okná, plávajúce podlahy, dlažba, stierky, novšia kuchynská linka, interiérové dvere so zárubňami, zateplený bytový dom. V blízkosti Kaufland, Lidl, poliklinika, škola, škôlka... RK-REALITA ponúka na predaj 3-izbový byt na Klokočine-Murániho ul., OV, 65m2, 5.p.,rekonštrukcia- plastové okná, plávajúce podlahy, stierky, kúpeľňa, novšia kuchynská linka, lógia, pivnica. Pri rýchlom jednaní možnosť zľavy. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel ... RK-REALITA ponúka na predaj 2-izbový byt na Klokočine- Čajkovského ul., OV, 4.p., 64m2, čiastočná rekonštrukcia, balkón, výborná ponuka. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729 Predáme 1-izbový byt v centre mesta v blízkosti OC Mlyny, OV, 37m2,čiastočná rekonštrukcia, zateplený bytový dom. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729 Ponúkame na predaj pekný 2-izbový byt na Chrenovej,Tr. A. Hlinku, OV, 64m2, 1.podlažie, Byt prešiel kompletná rekonštrukcia - bezpečnostné dvere, plastové okná, kvalitné vinylkorkové podlahy, stierky, murované jadro, kuchynská linka vrátane umývačky riadu, nová elektroinštalácia,zateplený bytový do ... Ponúkame na predaj svetlý 3-izbový byt na Chrenovej I, OV, 4.p., 80m2, plastové okná,drevené parkety, inak zachovalý pôvodný stav, balkón, nové výťahy, zateplený bytový dom. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729 RK-REALITA ponúka na predaj 3-izbový byt v centre, OV,5.p./6.p., kompletná rekonštrukcia-plastové okná, stierky, plávajúce podlahy, murované jadro, bezpečnostné dvere, ROOL DOOR, balkón, pivnica, zateplený bytový dom. Ďalšie inf. na tel. č: 0944 316 729",
//    Email = "[]",
//    Location = "Nitra",
//    Phone = "0944316729",
//    Portal = "http://reality.bazos.sk",
//    Price = "85 000 €",
//    Title = "3-IZBOVÝ BYT PRI KAUFLANDE, 78m2, BALKÓN",
//    Type = "Predaj",
//    Url = "http://reality.bazos.sk/inzerat/99367577/3-IZBOVY-BYT-PRI-KAUFLANDE-78m2-BALKON.php"
//},
//new Inzerat()
//{
//    ID = 3,
//    Author = "Štefan Kanižai",
//    Category = "Byty",
//    DateInserted = DateTime.Now,
//    Description = "4youReality Vám ponúka na predaj 3 izbový byt v obci Peder. Rozloha: byt+ spoločenské priestory+ obrovská pivnica+ loggia : 82m2 Poschodie : 1 .p. Výťah: Nie Loggia: Áno Popis nehnuteľnosti: - osobné vlastníctvo - vchodové dvere : pôvodne -okná : plastové - Exteriérové dvere: pôvodne - Interiérové dvere : pôvodné - pôvodná elektrina - IS: elektrika, voda, žumpa, plynová prípojka - oddelené WC a kúpeľná - stierky pôvodné - kúrenie na tuhé palivo ( kachľová pec ) - ohrev vody cez elektrický bojler - tehlový byt - k kuchyni prislúcha mala špajza - nízke mesačne náklady",
//    Email = "[]",
//    Location = "Prievidza",
//    Phone = "0918034090",
//    Portal = "http://reality.bazos.sk",
//    Price = "29 990 €",
//    Title = "Na predaj 3-izbovy byt -Peder",
//    Type = "Predaj",
//    Url = "http://reality.bazos.sk/inzerat/99367389/Na-predaj-3-izbovy-byt-Peder.php"
//},
//new Inzerat()
//{
//    ID = 4,
//    Author = "Ing.Božiková",
//    Category = "Byty",
//    DateInserted = DateTime.Now,
//    Description = "RK-REALITA ponúka na predaj veľkú garzónku na Klokočine- Škultétyho ul., OV, 24m2, plastové okno, inak zachovalý pôvodný stav, nové výťahy, zateplený bytový dom.Cena: 48.000€ + dohoda Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729 RK-REALITA ponúka na predaj 3-izbový byt na Klokočine-Murániho ul., OV, 65m2, 5.p.,rekonštrukcia- plastové okná, plávajúce podlahy, stierky, kúpeľňa, novšia kuchynská linka, lógia, pivnica. Pri rýchlom jednaní možnosť zľavy. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel ... Predáme 3-izbový byt na KLokočine -Novomeského ul., OV, 78m2, čiastočná rekonštrukcia- plastové okná, plávajúce podlahy, dlažba, stierky, novšia kuchynská linka, interiérové dvere so zárubňami, zateplený bytový dom. V blízkosti Kaufland, Lidl, poliklinika, škola, škôlka... RK-REALITA ponúka na predaj 2-izbový byt na Klokočine- Čajkovského ul., OV, 4.p., 64m2, čiastočná rekonštrukcia, balkón, výborná ponuka. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729 Predáme 1-izbový byt v centre mesta v blízkosti OC Mlyny, OV, 37m2,čiastočná rekonštrukcia, zateplený bytový dom. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729 Ponúkame na predaj pekný 2-izbový byt na Chrenovej,Tr. A. Hlinku, OV, 64m2, 1.podlažie, Byt prešiel kompletná rekonštrukcia - bezpečnostné dvere, plastové okná, kvalitné vinylkorkové podlahy, stierky, murované jadro, kuchynská linka vrátane umývačky riadu, nová elektroinštalácia,zateplený bytový do ... Ponúkame na predaj svetlý 3-izbový byt na Chrenovej I, OV, 4.p., 80m2, plastové okná,drevené parkety, inak zachovalý pôvodný stav, balkón, nové výťahy, zateplený bytový dom. Možnosť vybavenia najvýhodnejšieho hypotekárneho úveru. Ďalšie inf. na tel. č: 0944 316 729",
//    Email = "[]",
//    Location = "Nitra",
//    Phone = "0944316729",
//    Portal = "http://reality.bazos.sk",
//    Price = "Dohodou",
//    Title = "GARZÓNKA, KLOKOČINA, 24m2",
//    Type = "Predaj",
//    Url = "http://reality.bazos.sk/inzerat/99367189/GARZONKA-KLOKOCINA-24m2.php"
//},
//new Inzerat()
//{
//    ID = 5,
//    Author = "Mgr. Erika Majorošová - Diamond reality",
//    Category = "Byty",
//    DateInserted = DateTime.Now,
//    Description = "Ponúkame na predaj 4 izbový byt v novostavbe na Nábrežnej ulici v Prešove, s prekrásnym výhľadom na celé mesto . Byt sa nachádza asi 10 minút pešej chôdze od centra mesta , skolaudovaný. Byt sa predáva ako holo byt v štandarde zahŕňa: elektroinštalačné zriaďovacie predmety (, optické pripojenie , proti požiarne a bezpečnostné dvere , izolačné trojskla, 2x WC je zabudované typu Geberit, vlastný plynový kotol, v celom byte je podlahové kúrenie ,k bytu je jedno garážové státie . Byt pozostáva z chodby, predsiene, spálňa , izba, kúpeľňa s WC, WC, kuchyňa, obývačka +jedáleň, komora a veľká terasa . V cene bytu je obytná plocha je 83 m2 pivnica a garážové státie. Bytový dom je postavený z pálenej tehly + zateplený 15 cm minerálna vlna, aj preto sú maximálne náklady aj s elektrikou do 100 € na mesiac. V blízkosti bytu je kompletná občianska vybavenosť školy, škôlky, obchody atď.. . je to pekná a obľúbená lokalita na bývanie. Viac informácii na tel. čísle 0940883454 PhDr. Erika Majorošová V prípade, ak máte záujem o kúpu alebo predaj nehnuteľnosti, pozrite si prosím ponuku našej kancelárie, kde nájdete najširšiu ponuku realít na trhu. Samozrejmosťou je kompletný právny servis a právna istota zabezpečená advokátskou kanceláriou. Taktiež Vám radi pomôžeme s hypotékou prostredníctvom nášho interného finančného centra, ktoré Vám sprostredkuje poskytnutie najlepšieho úroku na trhu. V oblasti realít sme schopní pomôcť Vám Vaše dnešné sny premeniť na zajtrajšiu skutočnosť. Pre informácie o predmetnej nehnuteľnosti kontaktujte prosím tel. č. 0940 883454 PhDr. Erika Majorošová Protějovská ulica, Sídlisko 3 v Prešove, 3 izbák na predaj - 3 izbový - 72 m2 - murované jadro - lodžia a klíma - podlahy - plastové okná - 5. poschodie - 2x výťah, prerobená bytovka - interiér, exteriér (r.2010) - presvetlený, orientácia V a J - nové stupačky a radiátory - murovaný ša ... Ponúkame na predaj posledný luxusný penthouse 3 izbový byt v novostavbe na Nábrežnej ulici v širšom centre mesta Prešov, s prekrásnym výhľadom na celé mesto. Byt sa nachádza asi 10 minút pešej chôdze od centra mesta predpokladaná kolaudácia 12/18.Je to luxusná nehnuteľnosť ktorá splní všetky očakáva ... Ponúkame na predaj 2 izbový byt v novostavbe na Nábrežnej ulici v Prešove, s prekrásnym výhľadom na celé mesto . Byt sa nachádza asi 10 minút pešej chôdze od centra mesta predpokladaná kolaudácia 8/19. Byt sa predáva ako holo byt v štandarde zahŕňa: elektroinštalačné zriaďovacie predmety ( optické p ... Ponúkame na predaj 2 izbový byt v novostavbe na Nábrežnej ulici v Prešove, s prekrásnym výhľadom na celé mesto . Byt sa nachádza asi 10 minút pešej chôdze od centra mesta predpokladaná kolaudácia 8/19. Byt sa predáva ako holo byt v štandarde zahŕňa: elektroinštalačné zriaďovacie predmety (, optické ... Ponúkame na predaj 3 izbový byt v novostavbe na Nábrežnej ulici v Prešove, s prekrásnym výhľadom na celé mesto . Byt sa nachádza asi 10 minút pešej chôdze od centra mesta predpokladaná kolaudácia 8/19. Byt sa predáva ako holo byt v štandarde zahŕňa: elektroinštalačné zriaďovacie predmety ( sporákové ... Ponúkame na predaj 4 izbový mezometový byt v novostavbe v širšom centre mesta Pod Táborom v Prešove, s prekrásnym výhľadom . Byt sa nachádza asi 5 minút pešej chôdze od centra mesta, sú skolaudované a pripravené na predaj . Byt sa predáva ako holo byt v štandarde zahŕňa: elektroinštalačné zriaďova ...",
//    Email = "[]",
//    Location = "Prešov",
//    Phone = "0940883454",
//    Portal = "http://reality.bazos.sk",
//    Price = "210 000 €",
//    Title = "Na predaj 4 izbový veľkorysý byt v novostavbe v širšom centr",
//    Type = "Predaj",
//    Url = "http://reality.bazos.sk/inzerat/99366925/Na-predaj-4-izbovy-velkorysy-byt-v-novostavbe-v-sirsom-centre-mesta-s-.php"
//}
//                );
//                context.SaveChanges();
            }
        }
    }
}
