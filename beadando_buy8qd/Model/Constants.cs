using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Beadando.Model
{
    public  class Constants
    {


       

        //public const int numberOfElementsInAHorizontalRow_start = 10;
        //public const int numberOfElementsInAVerticalRow_start = 5;
        public const int puppetStartDiameter = 30;

        public static string[] playerTokens = { "NIK", "KVK", "RKK" };

        public static string[] nik_targyak = new string[] { "Programozás", "Digitális technika", "Adatbázisok" };
        public static string[] kando_targyak = new string[] { "Villamosságtan", "Elektronika", "Irányítástechnika" };
        public static string[] rejto_targyak = new string[] { "Formatervezés", "Művészeti ismeretek", "Ábrázoló Geometria" };

        #region EventCards
        public static string[] hallgatoevents = {"KOCKA NAPOK\nHa NIK-es vagy, kapsz egy tárgyat ingyen!", " KANDÓS IFJÚSÁGI NAPOK\nHa KVK-s vagy, kapsz egy tárgyat ingyen!",
            "REJTŐ NAPOK\nHa Rejtős vagy, kapsz egy tárgyat ingyen!", "TÖKÖS LEGÉNY\nEgy véletlenül kiválasztott helyről folytatod a játékot!" };
        public static string[] hallgatoevents_nevek = {"event_nik", "event_kando", "event_rejto", "tokos"};
        public static string[] neptunMessages = {"A tanulmányi osztály nyitvatartása előre nem látható okok miatt szünetel\nEGY KÖRBŐL KIMARADSZ",
        "Megkezdődött a diákmatricák kiadása a következő félévre\nLÉPJ A START MEZŐRE",
            $"Programozás III tantárgyának tanulmányi státusza változtatásra került. ’Még nem meghatározott’ státuszról ’Aláírva’ " +
                $"státuszra Kovács András módosította {DateTime.Today.Year}.{DateTime.Today.Month}.{DateTime.Today.Day} dátummal\nGRATULÁLUNK, NYERTÉL!!!",
            "Befizetés a gyújtószámlára\nFizess 7000-et!"
        };
        public static int[] go = { 1, 2, 3, 5 }; 
        public static string randi = "Pároddal töltötted a délutánt (is)\nFIZESS 5000-ET";
        public static string uv = "Alpót\nFIZESS 4000-ET"; //TODO change uv card to alpót
        public static string einstein = "100,5-ös ÖCSI\nKAPSZ 20.000 ÖSZTÖNDÍJAT";
        public static string lead = "Letiltottak egy tárgyadról mert nem érted el a 20%-os küszöböt!";
        public static string enroll = "TÁRGYFELVÉTELI LEHETŐSÉG\nLehetőséged van megvenni egy tárgyadat!";
        public static string roll = "Dobhatsz mégegyszer";
        public static string start = "Pontosan érkeztél a következő félévbe!\nJUTALMAD 5000";
        public static string megajanlott = "Megajánlott jegy!\nJUTALMAD EGY TÁRGY INGYEN";
        #endregion
        //string keys for the cards

    }
}
