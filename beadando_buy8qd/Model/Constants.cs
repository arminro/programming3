// <copyright file="Constants.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System;

    public static class Constants
    {
        // public const int numberOfElementsInAHorizontalRow_start = 10;
        // public const int numberOfElementsInAVerticalRow_start = 5;
        private const string RandiValue = "Pároddal töltötted a délutánt (is)\nFIZESS 5000-ET";
        private const string Uv = "Alpót\nFIZESS 4000-ET"; // TODO change uv card to alpót
        private const string EinsteinValue = "100,5-ös ÖCSI\nKAPSZ 20.000 ÖSZTÖNDÍJAT";
        private const string LeadValue = "Letiltottak egy tárgyadról mert nem érted el a 20%-os küszöböt!";
        private const string EnrollValue = "TÁRGYFELVÉTELI LEHETŐSÉG\nLehetőséged van megvenni egy tárgyadat!";
        private const string RollValue = "Dobhatsz mégegyszer";
        private const string StartValue = "Pontosan érkeztél a következő félévbe!\nJUTALMAD 5000";
        private const string MegajanlottValue = "Megajánlott jegy!\nJUTALMAD EGY TÁRGY INGYEN";

        private const int PuppetStartDiameterValue = 30;

        private static int[] go = { 1, 2, 3, 5 };

        private static string[] playerTokens = { "NIK", "KVK", "RKK" };

        private static string[] nikTargyak = new string[] { "Programozás", "Digitális technika", "Adatbázisok" };
        private static string[] kandoTargyak = new string[] { "Villamosságtan", "Elektronika", "Irányítástechnika" };
        private static string[] rejtoTargyak = new string[] { "Formatervezés", "Művészeti ismeretek", "Ábrázoló Geometria" };
        private static string[] hallgatoevents =
        {
            "KOCKA NAPOK\nHa NIK-es vagy, kapsz egy tárgyat ingyen!", " KANDÓS IFJÚSÁGI NAPOK\nHa KVK-s vagy, kapsz egy tárgyat ingyen!",
            "REJTŐ NAPOK\nHa Rejtős vagy, kapsz egy tárgyat ingyen!", "TÖKÖS LEGÉNY\nEgy véletlenül kiválasztott helyről folytatod a játékot!"
        };

        private static string[] hallgatoeventsNevek = { "event_nik", "event_kando", "event_rejto", "tokos" };
        private static string[] neptunMessages =
        {
            "A tanulmányi osztály nyitvatartása előre nem látható okok miatt szünetel\nEGY KÖRBŐL KIMARADSZ",
        "Megkezdődött a diákmatricák kiadása a következő félévre\nLÉPJ A START MEZŐRE",
            $"Programozás III tantárgyának tanulmányi státusza változtatásra került. ’Még nem meghatározott’ státuszról ’Aláírva’ " +
                $"státuszra Kovács András módosította {DateTime.Today.Year}.{DateTime.Today.Month}.{DateTime.Today.Day} dátummal\nGRATULÁLUNK, NYERTÉL!!!",
            "Befizetés a gyújtószámlára\nFizess 7000-et!"
        };

        public static int PuppetStartDiameter
        {
            get
            {
                return PuppetStartDiameterValue;
            }
        }

        public static string[] PlayerTokens
        {
            get
            {
                return playerTokens;
            }
        }

        public static string[] Nik_Targyak
        {
            get
            {
                return nikTargyak;
            }
        }

        public static string[] Kando_Targyak
        {
            get
            {
                return kandoTargyak;
            }
        }

        public static string[] Rejto_Targyak
        {
            get
            {
                return rejtoTargyak;
            }
        }

        public static string[] Hallgatoevents
        {
            get
            {
                return hallgatoevents;
            }
        }

        public static string[] Hallgatoevents_Nevek
        {
            get
            {
                return hallgatoeventsNevek;
            }
        }

        public static string[] NeptunMessages
        {
            get
            {
                return neptunMessages;
            }
        }

        public static int[] Go
        {
            get
            {
                return go;
            }
        }

        public static string Randi
        {
            get
            {
                return RandiValue;
            }
        }

        public static string UV
        {
            get
            {
                return Uv;
            }
        }

        public static string Einstein
        {
            get
            {
                return EinsteinValue;
            }
        }

        public static string Lead
        {
            get
            {
                return LeadValue;
            }
        }

        public static string Enroll
        {
            get
            {
                return EnrollValue;
            }
        }

        public static string Roll
        {
            get
            {
                return RollValue;
            }
        }

        public static string Start
        {
            get
            {
                return StartValue;
            }
        }

        public static string Megajanlott
        {
            get
            {
                return MegajanlottValue;
            }
        }

        // string keys for the cards
    }
}
