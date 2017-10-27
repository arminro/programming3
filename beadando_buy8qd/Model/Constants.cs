using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando
{
    public  class Constants
    {
        public static class NormalCard
        {
            public static int height_start = 135; //180
            public static int width_start = 90; //120
        }


        public static class SquareCard
        {
            public static int widthHeight_start = 135; //180
        }

        public const int startPosition_start = 1065; //950
        public const int lowerHorizontalAlign_start = 550;

        public const int numberOfElementsInAHorizontalRow_start = 10;
        public const int numberOfElementsInAVerticalRow_start = 5;
        public const int puppetStartDiameter = 30;

        #region EventCards
        public static string[] hallgatoevents = {"TÖKÖS LEGÉNY\nEgy véletlenül kiválasztott helyről folytatod a játékot!",
        "KOCKA NAPOK\nHa NIK-es vagy, kapsz egy tárgyat ingyen!"};
        public static string[] neptunMessages = {"A tanulmányi osztály nyitvatartása előre nem látható okok miatt szünetel\nEGY KÖRBŐL KIMARADSZ",
        "Megkezdődött a diákmatricák kiadása a következő félévre\nLÉPJ A START MEZŐRE",
            $"Programozás III tantárgyának tanulmányi státusza változtatásra került. ’Még nem meghatározott’ státuszról ’Aláírva’ " +
                $"státuszra Kovács András módosította {DateTime.Today.Year}.{DateTime.Today.Month}.{DateTime.Today.Day} dátummal\nGRATULÁLUNK, NYERTÉL!!!",
            "Befizetés a gyújtószámlára\nFizess 5000-et!"
        };
        public static int[] go = { 1, 2, 3, 5 };
        public static string randi = "Pároddal töltötted a délutánt (is)\nFIZESS 5000-ET";
        public static string uv = "UV\nFIZESS 4000-ET";
        public static string einstein = "100,5-ös ÖCSI\nKAPSZ 20.000 ÖSZTÖNDÍJAT";
        public static string lead = "Letiltottak egy tárgyadról mert nem érted el a 20%-os küszöböt!";
        public static string enroll = "TÁRGYFELVÉTELI LEHETŐSÉG";
        public static string roll = "Dobhatsz mégegyszer";
        public static string start = "Üdv a következő félévben!\nJUTALMAD 5000";
        public static string megajanlott = "Megajánlott jegy!\nJUTALMAD EGY TÁRGY INGYEN";
        #endregion
        //string keys for the cards

    }
}
