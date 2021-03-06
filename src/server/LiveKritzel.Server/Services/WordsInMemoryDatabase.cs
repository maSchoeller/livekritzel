﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Services
{
    public class WordsInMemoryDatabase
    {
        private static readonly List<string> list = new List<string> {
            "Montagsmaler-Begriffe",
            "Achselschweiß",
            "Armdrücken",
            "Autofahren",
            "Arzt",
            "Baby",
            "Bär",
            "Blinklicht",
            "Boxer",
            "Brustschwimmen",
            "Bürgermeister",
            "Detektiv",
            "Dickkopf",
            "Dirigent",
            "Dompteur",
            "Einbrechereinen",
            "Fischangeln",
            "Eisbär",
            "Elvis",
            "Eskimo",
            "Fahrradkette",
            "Fee",
            "Federballspielen",
            "Fernseher",
            "Flaschenhals",
            "Floh",
            "Friseuse",
            "Frosch",
            "Fußballspielen",
            "Gabelstapler",
            "Geldwechseln",
            "Herzensbrecher",
            "Haarföhn",
            "Handschuh",
            "Indianerjoggen",
            "Känguru",
            "Kartenspielen",
            "Käsefüße",
            "Kellnerin",
            "Klobrille",
            "Knopf",
            "Kopfschmerzen",
            "Kuchenbacken",
            "Kuhglocke",
            "Lehrerin",
            "Matrose",
            "Maus",
            "Nasenbohrer",
            "Papst",
            "Pferd",
            "Politiker",
            "Putzfrau",
            "Reiterin",
            "Reißverschluss",
            "Rennfahrer",
            "Riese",
            "Scheibenwischer",
            "Schornsteinfeger",
            "Schwimmen",
            "sichschminken",
            "sichduschen",
            "sichsonnen",
            "Sonnenblume",
            "Spion",
            "Sumoringer",
            "Surfer",
            "Tänzerin",
            "Taube",
            "Telefonieren",
            "Zähneputzen",
            "Zahnarzt",
            "Zwerg",
            "Wohnung",
            "Musikband",
            "Löwe",
            "Zebra",
            "Park",
            "dich",
            "Maus",
            "Wecker",
            "Strand",
            "Karotte",
            "Fußball",
            "5-Gänge-Menü",
            "Apfel",
            "Tennisschläger",
            "Rolerblades",
            "Spritzpistole",
            "Fotoapparat",
            "Spielplatz",
            "Fahrrad",
            "Tastatur",
            "Joystick",
            "Hängematte",
            "Sonnenuntergang",
            "Ritter",
            "Flohmarkt",
            "Eifelturm",
            "BrandenburgerTor",
            "Italien",
            "Autoschlüssel",
            "Handball",
            "Schiffskoch",
            "Baumhaus",
            "Blütenhonig",
            "Hammerhai",
            "Traum",
            "Tischfußball",
            "Sternbild",
            "Straußenei",
            "Blumenstrauß",
            "Arztkoffer",
            "Kopfnuss",
            "Oase",
            "Schwertfisch",
            "Kastanienbaum",
            "Warteschlange",
            "Brillenschlange",
            "Zaunkönig",
            "Saturn",
            "Gitarre",
            "Tankstelle",
            "Fernbedienung",
            "Rasierschaum",
            "Laderampe",
            "Lichterkette",
            "Rauhfasertapete",
            "Tannenbaumständer",
            "Siebträgermaschine",
            "Lampenfassung",
            "Schneeketten",
            "Konfettiregen",
            "Geschenkpapier",
            "Wollschal",
            "Schleifenbank",
            "Rundumleuchte",
            "Kugelbahn",
            "Schaukelpferd",
            "Verhaltensmuster",
            "Handcreme",
            "Räuchermännchen",
            "Querschläger",
            "Adventskalender",
            "Brillenputztuch",
            "Hauptgewinn",
            "Wäscheklammer",
            "Spiegelreflexkamera",
            "Pralinenschachtel",
            "Drehbuch",
            "Kinderstuhl",
            "Sonnenblumenöl",
            "Christbaumkugel",
            "Zungenkuss",
            "Ladegerät",
            "Schreihals",
            "Glühwein",
            "Osterhase",
            "Teppichklopfer",
            "Schokokringel",
            "Vollpfosten",
            "Sprudelwasser",
            "Kichstarter",
            "Eierlikör",
            "Neujahresgruß",
            "Glatteis",
            "Weihnachtsgans",
            "Lebkuchenherz",
            "Trostpreis",
            "Versuchskaninchen",
            "Regalboden",
            "Mistelzweig",
            "Hebamme",
            "Manschettenknopf",
            "Schneegestöber",
            "Renntier",
            "Winterstiefel",
            "Flussufer",
            "Pfauenfeder",
            "Ohrensessel",
            "Verlobungsring",
            "Nassrasur",
            "Garagentür",
            "Weihnachtspost",
            "Glasreiniger",
            "Intimpiercing",
            "Gießkanne",
            "Hackebeil",
            "Tiefkühlfach",
            "Treppenhaus",
            "Fahrradschlauch",
            "Kreißsaal",
            "Mitesser",
            "Armdrücken",
            "Armleuchter",
            "Auto",
            "Aalglatt",
            "Aasgeier",
            "Altklug",
            "Aschenbecher",
            "Autotür",
            "Autoschlüssel",
            "Arschbombe",
            "Angelrute",
            "Butterdose",
            "Buchrücken",
            "Buntstift",
            "Bademeister",
            "Barhocker",
            "Besenkammer",
            "Bienenstich",
            "Blattlaus",
            "Brecheisen",
            "Bankverbindung",
            "Druckerpatrone",
            "Dachterasse",
            "Dosenöffner",
            "Darmspülung",
            "Dübel",
            "Dönerbude",
            "Erbsenzähler",
            "Eselsbrücke",
            "Eisverkäufer",
            "Eisenbahn",
            "Elefantenhaut",
            "Elfenbeinküste",
            "Feuerwasser",
            "Feuchttücher",
            "Freudenhaus",
            "Fundgrube",
            "Fallobst",
            "Freiheitsstatue",
            "Federkissen",
            "Frauenarzt",
            "Himmelbett",
            "Hirtenstab",
            "Hängematte",
            "Haubentaucher",
            "HufeisenHirschgeweih",
            "Hippie",
            "Hundeleine",
            "Kuh",
            "Kettenkarussell",
            "Kautabak",
            "Kerzendocht",
            "Klaviertaste",
            "Käseglocke",
            "Kuchengabel",
            "Gürteltasche",
            "Glatteis",
            "Gemüsebeet",
            "Gorilla",
            "Gummiboot",
            "Giraffe",
            "Gartenschlauch",
            "Irrenhaus",
            "Imker",
            "Igel",
            "Internet",
            "Indianer",
            "Lochfrass",
            "Lichterkette",
            "Lesezeichen",
            "Luftmatratze",
            "Luftballon",
            "Nasenbär",
            "Navigationssystem",
            "Nussschale",
            "Naschkatze",
            "Nähmaschine",
            "Patronengürtel",
            "Pfandflasche",
            "Polizeiauto",
            "Portemonnaie",
            "Plattenspieler",
            "Motorroller",
            "Matrosenmütze",
            "Münzautomat",
            "Menschenaffe",
            "Mondschein",
            "Magnet",
            "Müllbeutel",
            "Raubüberfall",
            "Rollator",
            "Rübe",
            "Rosengarten",
            "Rundumleuchte",
            "Reisewecker",
            "Schlüsselloch",
            "Statue",
            "Schachbrett",
            "Schnürsenkel",
            "Seifenspender",
            "Schreibtischlampe",
            "Stehaufmännchen",
            "Suppenkasper",
            "Skilift",
            "Sandkasten",
            "Zahnfee",
            "Zigaretten",
            "Zahnarzt",
            "Zitronenbaum",
            "Zimmerpflanze",
            "Zungenkuss",
            "Zielscheibe",
            "Waffeleisen",
            "Wandregal",
            "Waschbecken",
            "Wasserball",
            "Wasserwaage"
        };

        public IReadOnlyList<string> Words => list;
    }
}
