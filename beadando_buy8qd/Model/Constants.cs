// <copyright file="Constants.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System;

    /// <summary>
    /// Stores the constants used during the initialization of the gameplay
    /// </summary>
    public static class Constants
    {
        // public const int numberOfElementsInAHorizontalRow_start = 10;
        // public const int numberOfElementsInAVerticalRow_start = 5;
        private const string RandiValue = "Pároddal töltötted a délutánt (is)\nFIZESS 5000-ET";
        private const string Alpot = "Alpót\nFIZESS 4000-ET";
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

        /// <summary>
        /// Gets the starting diameter of the player
        /// </summary>
        public static int PuppetStartDiameter
        {
            get
            {
                return PuppetStartDiameterValue;
            }
        }

        /// <summary>
        /// Gets the available names for the player puppets (nik, rejto, kando)
        /// </summary>
        public static string[] PlayerTokens
        {
            get
            {
                return playerTokens;
            }
        }

        /// <summary>
        /// Gets all the available subjects for the player if s/he is playing as a NIK-es
        /// </summary>
        public static string[] Nik_Targyak
        {
            get
            {
                return nikTargyak;
            }
        }

        /// <summary>
        /// Gets all the available subjects for the player if s/he is playing as a Kando-s
        /// </summary>
        public static string[] Kando_Targyak
        {
            get
            {
                return kandoTargyak;
            }
        }

        /// <summary>
        /// Gets all the available subjects for the player if s/he is playing as a Rejto-s
        /// </summary>
        public static string[] Rejto_Targyak
        {
            get
            {
                return rejtoTargyak;
            }
        }

        /// <summary>
        /// Gets the texts of all the different hallgato events
        /// </summary>
        public static string[] Hallgatoevents
        {
            get
            {
                return hallgatoevents;
            }
        }

        /// <summary>
        /// Gets the names of all the events
        /// </summary>
        public static string[] Hallgatoevents_Nevek
        {
            get
            {
                return hallgatoeventsNevek;
            }
        }

        /// <summary>
        /// Gets the texts of all the different variants of neptun messages
        /// </summary>
        public static string[] NeptunMessages
        {
            get
            {
                return neptunMessages;
            }
        }

        /// <summary>
        /// Gets the available numbers to randomly choose from in case the player stepped on the lépj előre card
        /// </summary>
        public static int[] Go
        {
            get
            {
                return go;
            }
        }

        /// <summary>
        /// Gets the text of the randi card
        /// </summary>
        public static string Randi
        {
            get
            {
                return RandiValue;
            }
        }

        /// <summary>
        /// Gets the text of the mighty alpot
        /// </summary>
        public static string AlpotProp
        {
            get
            {
                return Alpot;
            }
        }

        /// <summary>
        /// Gets the text of the einstein card
        /// </summary>
        public static string Einstein
        {
            get
            {
                return EinsteinValue;
            }
        }

        /// <summary>
        /// Gets the text of the lead card
        /// </summary>
        public static string Lead
        {
            get
            {
                return LeadValue;
            }
        }

        /// <summary>
        /// Gets the text for the correpsonfing card
        /// </summary>
        public static string Enroll
        {
            get
            {
                return EnrollValue;
            }
        }

        /// <summary>
        /// Gets the text for the correpsonfing card
        /// </summary>
        public static string Roll
        {
            get
            {
                return RollValue;
            }
        }

        /// <summary>
        /// Gets the text for the correpsonfing card
        /// </summary>
        public static string Start
        {
            get
            {
                return StartValue;
            }
        }

        /// <summary>
        /// Gets the text for the correpsonfing card
        /// </summary>
        public static string Megajanlott
        {
            get
            {
                return MegajanlottValue;
            }
        }
    }
}
