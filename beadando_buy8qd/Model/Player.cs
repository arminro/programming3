using Beadando.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Beadando
{
    class Player : Bindable
    {
        string puppetKey;
        Point currentposition;
        bool[] subjects; //array to store elements
        int currentCard; //storing the card the player sits on
        public Player(string puppetKey)
        {
            this.puppetKey = puppetKey;
            this.currentCard = 0;
            subjects = new bool[5]; //there are 5 subs
            for (int i = 0; i < subjects.Length; i++)
            {
                subjects[i] = false;
            }
        }

        public string PuppetKey
        {
            get
            {
                return puppetKey;
            }

          
        }

        public Point Currentposition
        {
            get
            {
                return currentposition;
            }

            set
            {
                currentposition = value;
                OnPropertyChanged();
            }
        }

        public bool[] Subjects
        {
            get
            {
                return subjects;
            }

        }

        public int CurrentCard
        {
            get
            {
                return currentCard;
            }

            set
            {
                currentCard = value;
            }
        }

        public void AddSubject(int subjectNumber)
        {
            subjects[subjectNumber] = true;
        }
        //public bool Subjects { get => subjects; set => subjects = value; }
    }
}
